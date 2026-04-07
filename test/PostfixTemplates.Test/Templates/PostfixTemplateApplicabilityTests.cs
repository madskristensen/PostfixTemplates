using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PostfixTemplateApplicabilityTests
{
    private static SemanticModel GetSemanticModel(string code)
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        return compilation.GetSemanticModel(tree);
    }

    private static ITypeSymbol GetTypeSymbol(string typeName)
    {
        var code = $@"
class Test {{
    void Method() {{
        {typeName} x = default;
    }}
}}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddReferences(MetadataReference.CreateFromFile(typeof(System.Collections.Generic.List<>).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel semanticModel = compilation.GetSemanticModel(tree);

        SyntaxNode root = tree.GetRoot();
        VariableDeclarationSyntax variableDeclaration = root.DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax>()
            .First();

        return semanticModel.GetTypeInfo(variableDeclaration.Type).Type;
    }

    [TestMethod]
    public void IfTemplate_ApplicableToBoolean()
    {
        var template = new IfTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");

        Assert.IsTrue(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToString()
    {
        var template = new IfTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsFalse(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToInt()
    {
        var template = new IfTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void NullTemplate_ApplicableToString()
    {
        var template = new NullTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void NullTemplate_NotApplicableToBoolean()
    {
        var template = new NullTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");

        Assert.IsFalse(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void NullTemplate_NotApplicableToInt()
    {
        var template = new NullTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void NullTemplate_ApplicableToNullableInt()
    {
        var template = new NullTemplate();
        ITypeSymbol nullableIntType = GetTypeSymbol("int?");

        Assert.IsTrue(template.IsApplicableToType(nullableIntType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToBoolean()
    {
        var template = new ForEachTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");

        Assert.IsFalse(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToInt()
    {
        var template = new ForEachTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToBoolean()
    {
        var template = new VarTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");

        Assert.IsTrue(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToString()
    {
        var template = new VarTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToInt()
    {
        var template = new VarTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsTrue(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void ReturnTemplate_ApplicableToAny()
    {
        var template = new ReturnTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");
        ITypeSymbol stringType = GetTypeSymbol("string");
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsTrue(template.IsApplicableToType(boolType));
        Assert.IsTrue(template.IsApplicableToType(stringType));
        Assert.IsTrue(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void ForEachTemplate_ApplicableToArray()
    {
        var template = new ForEachTemplate();
        ITypeSymbol arrayType = GetTypeSymbol("int[]");

        Assert.IsTrue(template.IsApplicableToType(arrayType));
    }

    [TestMethod]
    public void ForEachTemplate_ApplicableToListOfT()
    {
        var template = new ForEachTemplate();
        ITypeSymbol listType = GetTypeSymbol("System.Collections.Generic.List<int>");

        Assert.IsTrue(template.IsApplicableToType(listType));
    }

    [TestMethod]
    public void ForEachTemplate_ApplicableToString()
    {
        var template = new ForEachTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void ThrowTemplate_ApplicableToException()
    {
        var template = new ThrowTemplate();
        ITypeSymbol exceptionType = GetTypeSymbol("System.Exception");

        Assert.IsTrue(template.IsApplicableToType(exceptionType));
    }

    [TestMethod]
    public void ThrowTemplate_ApplicableToDerivedExceptionType()
    {
        var template = new ThrowTemplate();
        ITypeSymbol argumentExceptionType = GetTypeSymbol("System.ArgumentException");

        Assert.IsTrue(template.IsApplicableToType(argumentExceptionType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToString()
    {
        var template = new ThrowTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsFalse(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToInt()
    {
        var template = new ThrowTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void WhileTemplate_ApplicableToBoolean()
    {
        var template = new WhileTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");

        Assert.IsTrue(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void WhileTemplate_NotApplicableToInt()
    {
        var template = new WhileTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void ForTemplate_ApplicableToArray()
    {
        var template = new ForTemplate();
        ITypeSymbol arrayType = GetTypeSymbol("int[]");

        Assert.IsTrue(template.IsApplicableToType(arrayType));
    }

    [TestMethod]
    public void ForTemplate_NotApplicableToInt()
    {
        var template = new ForTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void ForRTemplate_ApplicableToArray()
    {
        var template = new ForRTemplate();
        ITypeSymbol arrayType = GetTypeSymbol("int[]");

        Assert.IsTrue(template.IsApplicableToType(arrayType));
    }

    [TestMethod]
    public void ForRTemplate_NotApplicableToInt()
    {
        var template = new ForRTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void SwitchTemplate_ApplicableToBoolean()
    {
        var template = new SwitchTemplate();
        ITypeSymbol boolType = GetTypeSymbol("bool");

        Assert.IsTrue(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void SwitchTemplate_ApplicableToString()
    {
        var template = new SwitchTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void LockTemplate_ApplicableToString()
    {
        var template = new LockTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void LockTemplate_NotApplicableToInt()
    {
        var template = new LockTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void UsingTemplate_ApplicableToDisposableType()
    {
        var template = new UsingTemplate();
        ITypeSymbol streamType = GetTypeSymbol("System.IO.StreamReader");

        Assert.IsTrue(template.IsApplicableToType(streamType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToString()
    {
        var template = new UsingTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsFalse(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToList()
    {
        var template = new UsingTemplate();
        ITypeSymbol listType = GetTypeSymbol("System.Collections.Generic.List<int>");

        Assert.IsFalse(template.IsApplicableToType(listType));
    }

    [TestMethod]
    public void AwaitTemplate_ApplicableToTask()
    {
        var template = new AwaitTemplate();
        ITypeSymbol taskType = GetTypeSymbol("System.Threading.Tasks.Task");

        Assert.IsTrue(template.IsApplicableToType(taskType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToString()
    {
        var template = new AwaitTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsFalse(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToList()
    {
        var template = new AwaitTemplate();
        ITypeSymbol listType = GetTypeSymbol("System.Collections.Generic.List<int>");

        Assert.IsFalse(template.IsApplicableToType(listType));
    }

    [TestMethod]
    public void AllTemplates_ApplicableWhenTypeIsNull()
    {
        // When we can't determine the type, templates should still show
        foreach (PostfixTemplate? template in PostfixTemplate.All)
        {
            Assert.IsTrue(template.IsApplicableToType(null), $"Template '{template.Name}' should be applicable when type is null");
        }
    }

    [TestMethod]
    public void AllTemplates_HaveSuffix()
    {
        foreach (PostfixTemplate? template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Suffix), $"Template '{template.Name}' should have a suffix");
        }
    }

    [TestMethod]
    public void RequiresValueExpression_TrueByDefault()
    {
        // Most templates should require a value expression
        Assert.IsTrue(new NullTemplate().RequiresValueExpression);
        Assert.IsTrue(new NotNullTemplate().RequiresValueExpression);
        Assert.IsTrue(new ReturnTemplate().RequiresValueExpression);
        Assert.IsTrue(new VarTemplate().RequiresValueExpression);
    }

    [TestMethod]
    public void RequiresValueExpression_FalseForTypeTemplates()
    {
        // Templates designed for type names must not require a value expression
        Assert.IsFalse(new InjectTemplate().RequiresValueExpression);
        Assert.IsFalse(new NewTemplate().RequiresValueExpression);
        Assert.IsFalse(new TypeofTemplate().RequiresValueExpression);
    }

    [TestMethod]
    public void ParseTemplate_ApplicableToString()
    {
        var template = new ParseTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void ParseTemplate_NotApplicableToInt()
    {
        var template = new ParseTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void ParseTemplate_NotApplicableToList()
    {
        var template = new ParseTemplate();
        ITypeSymbol listType = GetTypeSymbol("System.Collections.Generic.List<int>");

        Assert.IsFalse(template.IsApplicableToType(listType));
    }

    [TestMethod]
    public void TryParseTemplate_ApplicableToString()
    {
        var template = new TryParseTemplate();
        ITypeSymbol stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void TryParseTemplate_NotApplicableToInt()
    {
        var template = new TryParseTemplate();
        ITypeSymbol intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void LockTemplate_ApplicableToList()
    {
        var template = new LockTemplate();
        ITypeSymbol listType = GetTypeSymbol("System.Collections.Generic.List<int>");

        Assert.IsTrue(template.IsApplicableToType(listType));
    }
}
