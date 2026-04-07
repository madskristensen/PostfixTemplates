using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class SelTemplateTests
{
    private readonly SelTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsSel()
    {
        Assert.AreEqual("sel", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_ReturnsExpressionUnchanged()
    {
        var result = _template.GetTransformedText("selectedExpression", "    ");

        Assert.AreEqual("selectedExpression", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("obj.Method() + other", "    ");

        Assert.AreEqual("obj.Method() + other", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("expr", "    ");
        var resultB = _template.GetTransformedText("expr", "        ");

        Assert.AreEqual(resultA, resultB, "SelTemplate output should not depend on indent");
    }
}
