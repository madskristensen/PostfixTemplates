using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PostfixTemplates.Completion;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

/// <summary>
/// Sanity checks verifying:
/// 1. Async/iterator context detection helpers in RoslynExpressionHelper
/// 2. Every template's SelectionPlaceholder appears in its output
/// </summary>
[TestClass]
public class ContextAndOutputSanityTests
{
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
