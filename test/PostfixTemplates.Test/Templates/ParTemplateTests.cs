using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ParTemplateTests
{
    private readonly ParTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsPar()
    {
        Assert.AreEqual("par", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleExpression()
    {
        var result = _template.GetTransformedText("a + b", "    ");

        Assert.AreEqual("(a + b)", result);
    }

    [TestMethod]
    public void GetTransformedText_Variable()
    {
        var result = _template.GetTransformedText("value", "    ");

        Assert.AreEqual("(value)", result);
    }

    [TestMethod]
    public void GetTransformedText_NestedExpression()
    {
        var result = _template.GetTransformedText("x * y + z", "    ");

        Assert.AreEqual("(x * y + z)", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("expr", "    ");
        var resultB = _template.GetTransformedText("expr", "        ");

        Assert.AreEqual(resultA, resultB, "ParTemplate output should not depend on indent");
    }
}
