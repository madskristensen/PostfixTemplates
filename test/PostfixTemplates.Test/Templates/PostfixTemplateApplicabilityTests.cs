using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PostfixTemplateApplicabilityTests
{
    private static SemanticModel GetSemanticModel(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create("Test")
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
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddReferences(MetadataReference.CreateFromFile(typeof(System.Collections.Generic.List<>).Assembly.Location))
            .AddSyntaxTrees(tree);
        var semanticModel = compilation.GetSemanticModel(tree);

        var root = tree.GetRoot();
        var variableDeclaration = root.DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax>()
            .First();

        return semanticModel.GetTypeInfo(variableDeclaration.Type).Type;
    }

    [TestMethod]
    public void IfTemplate_ApplicableToBoolean()
    {
        var template = new IfTemplate();
        var boolType = GetTypeSymbol("bool");

        Assert.IsTrue(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToString()
    {
        var template = new IfTemplate();
        var stringType = GetTypeSymbol("string");

        Assert.IsFalse(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToInt()
    {
        var template = new IfTemplate();
        var intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void NullTemplate_ApplicableToString()
    {
        var template = new NullTemplate();
        var stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void NullTemplate_NotApplicableToBoolean()
    {
        var template = new NullTemplate();
        var boolType = GetTypeSymbol("bool");

        Assert.IsFalse(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void NullTemplate_NotApplicableToInt()
    {
        var template = new NullTemplate();
        var intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void NullTemplate_ApplicableToNullableInt()
    {
        var template = new NullTemplate();
        var nullableIntType = GetTypeSymbol("int?");

        Assert.IsTrue(template.IsApplicableToType(nullableIntType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToBoolean()
    {
        var template = new ForEachTemplate();
        var boolType = GetTypeSymbol("bool");

        Assert.IsFalse(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToInt()
    {
        var template = new ForEachTemplate();
        var intType = GetTypeSymbol("int");

        Assert.IsFalse(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToBoolean()
    {
        var template = new VarTemplate();
        var boolType = GetTypeSymbol("bool");

        Assert.IsTrue(template.IsApplicableToType(boolType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToString()
    {
        var template = new VarTemplate();
        var stringType = GetTypeSymbol("string");

        Assert.IsTrue(template.IsApplicableToType(stringType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToInt()
    {
        var template = new VarTemplate();
        var intType = GetTypeSymbol("int");

        Assert.IsTrue(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void ReturnTemplate_ApplicableToAny()
    {
        var template = new ReturnTemplate();
        var boolType = GetTypeSymbol("bool");
        var stringType = GetTypeSymbol("string");
        var intType = GetTypeSymbol("int");

        Assert.IsTrue(template.IsApplicableToType(boolType));
        Assert.IsTrue(template.IsApplicableToType(stringType));
        Assert.IsTrue(template.IsApplicableToType(intType));
    }

    [TestMethod]
    public void AllTemplates_ApplicableWhenTypeIsNull()
    {
        // When we can't determine the type, templates should still show
        foreach (var template in PostfixTemplate.All)
        {
            Assert.IsTrue(template.IsApplicableToType(null), $"Template '{template.Name}' should be applicable when type is null");
        }
    }

    [TestMethod]
    public void AllTemplates_HaveSuffix()
    {
        foreach (var template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Suffix), $"Template '{template.Name}' should have a suffix");
        }
    }
}
