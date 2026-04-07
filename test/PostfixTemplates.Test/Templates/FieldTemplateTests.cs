using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class FieldTemplateTests
{
    private readonly FieldTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsField()
    {
        Assert.AreEqual("field", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleExpression()
    {
        var result = _template.GetTransformedText("value", "    ");

        Assert.AreEqual("_field = value;", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("CreateService()", "    ");

        Assert.AreEqual("_field = CreateService();", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("new List<int>()", "    ");

        Assert.AreEqual("_field = new List<int>();", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("value", "    ");
        var resultB = _template.GetTransformedText("value", "        ");

        Assert.AreEqual(resultA, resultB, "FieldTemplate output should not depend on indent");
    }
}
