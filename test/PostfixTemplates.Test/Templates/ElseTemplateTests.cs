using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ElseTemplateTests
{
    private readonly ElseTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsElse()
    {
        Assert.AreEqual("else", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleIdentifier_NegatesWithoutParentheses()
    {
        var result = _template.GetTransformedText("myBool", "    ");

        var expected = $"if (!myBool){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression_NegatesWithParentheses()
    {
        var result = _template.GetTransformedText("a && b", "    ");

        var expected = $"if (!(a && b)){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_ComparisonOperator_NegatesWithParentheses()
    {
        var result = _template.GetTransformedText("x > 5", "    ");

        var expected = $"if (!(x > 5)){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall_NegatesWithoutParentheses()
    {
        var result = _template.GetTransformedText("IsReady()", "    ");

        var expected = $"if (!IsReady()){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("flag", "        ");

        var expected = $"if (!flag){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
