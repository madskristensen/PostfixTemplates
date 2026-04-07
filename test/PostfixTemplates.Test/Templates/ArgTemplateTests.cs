using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ArgTemplateTests
{
    private readonly ArgTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsArg()
    {
        Assert.AreEqual("arg", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleExpression()
    {
        var result = _template.GetTransformedText("value", "    ");

        Assert.AreEqual("Method(value)", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("GetValue()", "    ");

        Assert.AreEqual("Method(GetValue())", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("obj.Property", "    ");

        Assert.AreEqual("Method(obj.Property)", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("x", "    ");
        var resultB = _template.GetTransformedText("x", "        ");

        Assert.AreEqual(resultA, resultB, "ArgTemplate output should not depend on indent");
    }
}
