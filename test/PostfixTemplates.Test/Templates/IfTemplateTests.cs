using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class IfTemplateTests
{
    private readonly IfTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsIf()
    {
        Assert.AreEqual("if", _template.Name);
    }

    [TestMethod]
    public void Description_IsNotEmpty()
    {
        Assert.IsFalse(string.IsNullOrEmpty(_template.Description));
    }

    [TestMethod]
    public void Example_IsNotEmpty()
    {
        Assert.IsFalse(string.IsNullOrEmpty(_template.Example));
    }

    [TestMethod]
    public void GetTransformedText_SimpleIdentifier()
    {
        var result = _template.GetTransformedText("myBool", "    ");

        var expected = $"if (myBool){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("a && b", "    ");

        var expected = $"if (a && b){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("IsReady()", "    ");

        var expected = $"if (IsReady()){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MemberAccess()
    {
        var result = _template.GetTransformedText("obj.IsValid", "    ");

        var expected = $"if (obj.IsValid){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("x", "        ");

        var expected = $"if (x){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_EmptyIndent()
    {
        var result = _template.GetTransformedText("x", "");

        var expected = $"if (x){Environment.NewLine}{{{Environment.NewLine}    {Environment.NewLine}}}";
        Assert.AreEqual(expected, result);
    }
}
