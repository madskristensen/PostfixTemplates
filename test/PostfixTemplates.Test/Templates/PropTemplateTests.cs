using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PropTemplateTests
{
    private readonly PropTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsProp()
    {
        Assert.AreEqual("prop", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleExpression()
    {
        var result = _template.GetTransformedText("value", "    ");

        Assert.AreEqual("Property = value;", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("Calculate()", "    ");

        Assert.AreEqual("Property = Calculate();", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("obj.GetName()", "    ");

        Assert.AreEqual("Property = obj.GetName();", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("value", "    ");
        var resultB = _template.GetTransformedText("value", "        ");

        Assert.AreEqual(resultA, resultB, "PropTemplate output should not depend on indent");
    }
}
