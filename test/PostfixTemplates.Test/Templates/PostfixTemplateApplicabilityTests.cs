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
}
