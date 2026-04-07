using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PostfixTemplates.Completion;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

/// <summary>
/// Comprehensive sanity checks verifying:
/// 1. Every template's context flags (RequiresAsyncContext, RequiresIteratorContext, RequiresValueExpression)
/// 2. Async/iterator context detection helpers in RoslynExpressionHelper
/// 3. Every template's GetTransformedText produces parseable C#
/// </summary>
[TestClass]
public class ContextAndOutputSanityTests
{
    // ===================================================================
    // Context flags: RequiresAsyncContext
    // ===================================================================
    [TestMethod]
    public void OnlyAwaitTemplate_RequiresAsyncContext()
    {
        foreach (PostfixTemplate template in PostfixTemplate.All)
        {
            if (template.Name == "await")
            {
                Assert.IsTrue(template.RequiresAsyncContext, $"'{template.Name}' should require async context");
            }
            else
            {
                Assert.IsFalse(template.RequiresAsyncContext, $"'{template.Name}' should NOT require async context");
            }
        }
    }

    // ===================================================================
    // Context flags: RequiresIteratorContext
    // ===================================================================
    [TestMethod]
    public void OnlyYieldTemplate_RequiresIteratorContext()
    {
        foreach (PostfixTemplate template in PostfixTemplate.All)
        {
            if (template.Name == "yield")
            {
                Assert.IsTrue(template.RequiresIteratorContext, $"'{template.Name}' should require iterator context");
            }
            else
            {
                Assert.IsFalse(template.RequiresIteratorContext, $"'{template.Name}' should NOT require iterator context");
            }
        }
    }

    // ===================================================================
    // Context flags: RequiresValueExpression
    // ===================================================================
    [TestMethod]
    public void OnlyTypeTemplates_DoNotRequireValueExpression()
    {
        var typeOnlyTemplates = new HashSet<string> { "new", "typeof", "inject" };

        foreach (PostfixTemplate template in PostfixTemplate.All)
        {
            if (typeOnlyTemplates.Contains(template.Name))
            {
                Assert.IsFalse(template.RequiresValueExpression, $"'{template.Name}' should NOT require value expression (it operates on types)");
            }
            else
            {
                Assert.IsTrue(template.RequiresValueExpression, $"'{template.Name}' should require value expression");
            }
        }
    }

    // ===================================================================
    // IsInAsyncContext detection
    // ===================================================================
    [TestMethod]
    public void IsInAsyncContext_TrueInsideAsyncMethod()
    {
        var code = @"
using System.Threading.Tasks;
class C {
    async Task M() {
        var x = 1;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        var position = code.IndexOf("var x");

        Assert.IsTrue(RoslynExpressionHelper.IsInAsyncContext(tree, position));
    }

    [TestMethod]
    public void IsInAsyncContext_FalseInsideNonAsyncMethod()
    {
        var code = @"
class C {
    void M() {
        var x = 1;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        var position = code.IndexOf("var x");

        Assert.IsFalse(RoslynExpressionHelper.IsInAsyncContext(tree, position));
    }

    [TestMethod]
    public void IsInAsyncContext_TrueInsideAsyncLambda()
    {
        var code = @"
using System;
using System.Threading.Tasks;
class C {
    void M() {
        Func<Task> f = async () => {
            var x = 1;
        };
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        var position = code.IndexOf("var x");

        Assert.IsTrue(RoslynExpressionHelper.IsInAsyncContext(tree, position));
    }

    [TestMethod]
    public void IsInAsyncContext_FalseInsideNonAsyncLambda()
    {
        var code = @"
using System;
class C {
    void M() {
        Action f = () => {
            var x = 1;
        };
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        var position = code.IndexOf("var x");

        Assert.IsFalse(RoslynExpressionHelper.IsInAsyncContext(tree, position));
    }

    [TestMethod]
    public void IsInAsyncContext_FalseWhenTreeIsNull()
    {
        Assert.IsFalse(RoslynExpressionHelper.IsInAsyncContext(null, 0));
    }

    [TestMethod]
    public void IsInAsyncContext_FalseInsideAsyncMethodParent_ButInsideNonAsyncLocalFunction()
    {
        var code = @"
using System.Threading.Tasks;
class C {
    async Task M() {
        void LocalFunc() {
            var x = 1;
        }
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        var position = code.IndexOf("var x");

        Assert.IsFalse(RoslynExpressionHelper.IsInAsyncContext(tree, position));
    }

    [TestMethod]
    public void IsInAsyncContext_TrueInsideAsyncLocalFunction()
    {
        var code = @"
using System.Threading.Tasks;
class C {
    void M() {
        async Task LocalFunc() {
            var x = 1;
        }
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        var position = code.IndexOf("var x");

        Assert.IsTrue(RoslynExpressionHelper.IsInAsyncContext(tree, position));
    }

    // ===================================================================
    // IsInIteratorContext detection
    // ===================================================================
    [TestMethod]
    public void IsInIteratorContext_TrueInsideIEnumerableMethod()
    {
        var code = @"
using System.Collections.Generic;
class C {
    IEnumerable<int> M() {
        var x = 1;
        yield return x;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel model = compilation.GetSemanticModel(tree);
        var position = code.IndexOf("var x");

        Assert.IsTrue(RoslynExpressionHelper.IsInIteratorContext(tree, position, model));
    }

    [TestMethod]
    public void IsInIteratorContext_FalseInsideVoidMethod()
    {
        var code = @"
class C {
    void M() {
        var x = 1;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel model = compilation.GetSemanticModel(tree);
        var position = code.IndexOf("var x");

        Assert.IsFalse(RoslynExpressionHelper.IsInIteratorContext(tree, position, model));
    }

    [TestMethod]
    public void IsInIteratorContext_FalseInsideTaskMethod()
    {
        var code = @"
using System.Threading.Tasks;
class C {
    Task M() {
        var x = 1;
        return Task.CompletedTask;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel model = compilation.GetSemanticModel(tree);
        var position = code.IndexOf("var x");

        Assert.IsFalse(RoslynExpressionHelper.IsInIteratorContext(tree, position, model));
    }

    [TestMethod]
    public void IsInIteratorContext_FalseWhenTreeIsNull()
    {
        Assert.IsFalse(RoslynExpressionHelper.IsInIteratorContext(null, 0, null));
    }

    [TestMethod]
    public void IsInIteratorContext_TrueInsideIEnumeratorMethod()
    {
        var code = @"
using System.Collections;
class C {
    IEnumerator M() {
        var x = 1;
        yield return x;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel model = compilation.GetSemanticModel(tree);
        var position = code.IndexOf("var x");

        Assert.IsTrue(RoslynExpressionHelper.IsInIteratorContext(tree, position, model));
    }

    [TestMethod]
    public void IsInIteratorContext_FalseInsideLambdaInsideIteratorMethod()
    {
        var code = @"
using System;
using System.Collections.Generic;
class C {
    IEnumerable<int> M() {
        Action a = () => {
            var x = 1;
        };
        yield return 0;
    }
}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.CancellationToken);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel model = compilation.GetSemanticModel(tree);
        var position = code.IndexOf("var x");

        Assert.IsFalse(RoslynExpressionHelper.IsInIteratorContext(tree, position, model));
    }

    // ===================================================================
    // Output validity: every template's GetTransformedText produces parseable C#
    //
    // Some templates produce statements (e.g. if, return, var), others produce
    // bare expressions (e.g. not, par, await, cast). We try wrapping as a
    // statement first; if that fails we try as an expression statement.
    // ===================================================================
    private static bool ProducesParseableCode(PostfixTemplate template, string expression)
    {
        var transformed = template.GetTransformedText(expression, "        ");

        // Try as a statement inside a method body
        var statementCode = $@"
class C {{
    void M() {{
        {transformed}
    }}
}}";

        SyntaxTree stmtTree = CSharpSyntaxTree.ParseText(statementCode);
        var stmtErrors = stmtTree.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();

        if (stmtErrors.Count == 0)
        {
            return true;
        }

        // Some templates produce bare expressions (e.g. !myBool, (expr), await task).
        // Try wrapping with semicolon and assignment to validate as a statement.
        var exprCode = $@"
class C {{
    void M() {{
        var __result = {transformed};
    }}
}}";

        SyntaxTree exprTree = CSharpSyntaxTree.ParseText(exprCode);
        var exprErrors = exprTree.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();

        return exprErrors.Count == 0;
    }

    [TestMethod]
    public void IfTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new IfTemplate(), "myBool"));
    }

    [TestMethod]
    public void ElseTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ElseTemplate(), "myBool"));
    }

    [TestMethod]
    public void NotTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new NotTemplate(), "myBool"));
    }

    [TestMethod]
    public void WhileTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new WhileTemplate(), "condition"));
    }

    [TestMethod]
    public void ForEachTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ForEachTemplate(), "items"));
    }

    [TestMethod]
    public void ForTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ForTemplate(), "items"));
    }

    [TestMethod]
    public void ForRTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ForRTemplate(), "items"));
    }

    [TestMethod]
    public void VarTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new VarTemplate(), "GetValue()"));
    }

    [TestMethod]
    public void ReturnTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ReturnTemplate(), "result"));
    }

    [TestMethod]
    public void ThrowTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ThrowTemplate(), "new System.Exception()"));
    }

    [TestMethod]
    public void AwaitTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new AwaitTemplate(), "task"));
    }

    [TestMethod]
    public void LockTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new LockTemplate(), "syncRoot"));
    }

    [TestMethod]
    public void UsingTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new UsingTemplate(), "resource"));
    }

    [TestMethod]
    public void SwitchTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new SwitchTemplate(), "value"));
    }

    [TestMethod]
    public void ArgTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ArgTemplate(), "myValue"));
    }

    [TestMethod]
    public void CastTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new CastTemplate(), "obj"));
    }

    [TestMethod]
    public void ParTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ParTemplate(), "value"));
    }

    [TestMethod]
    public void FieldTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new FieldTemplate(), "GetValue()"));
    }

    [TestMethod]
    public void PropTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new PropTemplate(), "GetValue()"));
    }

    [TestMethod]
    public void ToTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ToTemplate(), "GetValue()"));
    }

    [TestMethod]
    public void NullTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new NullTemplate(), "obj"));
    }

    [TestMethod]
    public void NotNullTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new NotNullTemplate(), "obj"));
    }

    [TestMethod]
    public void ParseTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ParseTemplate(), "str"));
    }

    [TestMethod]
    public void TryParseTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new TryParseTemplate(), "str"));
    }

    [TestMethod]
    public void YieldTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new YieldTemplate(), "item"));
    }

    [TestMethod]
    public void NewTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new NewTemplate(), "System.Object"));
    }

    [TestMethod]
    public void TypeofTemplate_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new TypeofTemplate(), "int"));
    }

    [TestMethod]
    public void InjectTemplate_ProducesValidParameterListFragment()
    {
        // Inject produces a primary constructor parameter list fragment like "(IService dependency)".
        // This is designed for C# 12+ primary constructors and is a structural fragment,
        // not a standalone statement. Verify the output structure.
        var transformed = new InjectTemplate().GetTransformedText("IService", "    ");

        Assert.StartsWith("(", transformed, "Should start with opening parenthesis");
        Assert.EndsWith(")", transformed, "Should end with closing parenthesis");
        Assert.Contains("IService", transformed, "Should contain the type name");
        Assert.Contains("dependency", transformed, "Should contain the parameter name");
    }

    // ===================================================================
    // Output validity with complex expressions
    // ===================================================================
    [TestMethod]
    public void ElseTemplate_NegatedComplexExpression_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new ElseTemplate(), "a && b"));
    }

    [TestMethod]
    public void NotTemplate_NegatedComplexExpression_ProducesParseableOutput()
    {
        Assert.IsTrue(ProducesParseableCode(new NotTemplate(), "x > 5"));
    }

    // ===================================================================
    // SelectionPlaceholder: templates that declare one must contain it in output
    // ===================================================================
    [TestMethod]
    public void AllTemplatesWithPlaceholder_ContainPlaceholderInOutput()
    {
        foreach (PostfixTemplate template in PostfixTemplate.All)
        {
            if (template.SelectionPlaceholder == null)
            {
                continue;
            }

            var output = template.GetTransformedText("expr", "    ");
            Assert.Contains(
template.SelectionPlaceholder,
                output, $"Template '{template.Name}' declares placeholder '{template.SelectionPlaceholder}' but output does not contain it: {output}");
        }
    }

    public TestContext TestContext { get; set; }
}
