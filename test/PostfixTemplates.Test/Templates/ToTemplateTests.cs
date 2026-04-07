using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ToTemplateTests
{
    private readonly ToTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsTo()
    {
        Assert.AreEqual("to", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleExpression()
    {
        var result = _template.GetTransformedText("value", "    ");

        Assert.AreEqual("lvalue = value;", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("Calculate(x, y)", "    ");

        Assert.AreEqual("lvalue = Calculate(x, y);", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("obj.GetResult()", "    ");

        Assert.AreEqual("lvalue = obj.GetResult();", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("value", "    ");
        var resultB = _template.GetTransformedText("value", "        ");

        Assert.AreEqual(resultA, resultB, "ToTemplate output should not depend on indent");
    }
}
