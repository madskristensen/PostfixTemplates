using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class VarTemplateTests
{
    private readonly VarTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsVar()
    {
        Assert.AreEqual("var", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleExpression()
    {
        var result = _template.GetTransformedText("GetValue()", "    ");

        Assert.AreEqual("var x = GetValue();", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("obj.Method().Property", "    ");

        Assert.AreEqual("var x = obj.Method().Property;", result);
    }

    [TestMethod]
    public void GetTransformedText_Constructor()
    {
        var result = _template.GetTransformedText("new List<int>()", "    ");

        Assert.AreEqual("var x = new List<int>();", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("value", "    ");
        var resultB = _template.GetTransformedText("value", "        ");

        Assert.AreEqual(resultA, resultB, "VarTemplate output should not depend on indent");
    }
}
