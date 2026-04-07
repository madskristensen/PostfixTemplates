using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ReturnTemplateTests
{
    private readonly ReturnTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsReturn()
    {
        Assert.AreEqual("return", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleValue()
    {
        var result = _template.GetTransformedText("value", "    ");

        Assert.AreEqual("return value;", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("Calculate(x, y)", "    ");

        Assert.AreEqual("return Calculate(x, y);", result);
    }

    [TestMethod]
    public void GetTransformedText_NullLiteral()
    {
        var result = _template.GetTransformedText("null", "    ");

        Assert.AreEqual("return null;", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("x", "    ");
        var resultB = _template.GetTransformedText("x", "        ");

        Assert.AreEqual(resultA, resultB, "ReturnTemplate output should not depend on indent");
    }
}
