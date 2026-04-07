using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class YieldTemplateTests
{
    private readonly YieldTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsYield()
    {
        Assert.AreEqual("yield", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleValue()
    {
        var result = _template.GetTransformedText("item", "    ");

        Assert.AreEqual("yield return item;", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("ComputeNext()", "    ");

        Assert.AreEqual("yield return ComputeNext();", result);
    }

    [TestMethod]
    public void GetTransformedText_ComplexExpression()
    {
        var result = _template.GetTransformedText("items[i]", "    ");

        Assert.AreEqual("yield return items[i];", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("value", "    ");
        var resultB = _template.GetTransformedText("value", "        ");

        Assert.AreEqual(resultA, resultB, "YieldTemplate output should not depend on indent");
    }

    [TestMethod]
    public void RequiresIteratorContext_ReturnsTrue()
    {
        Assert.IsTrue(_template.RequiresIteratorContext);
    }

    [TestMethod]
    public void RequiresAsyncContext_ReturnsFalse()
    {
        Assert.IsFalse(_template.RequiresAsyncContext);
    }
}
