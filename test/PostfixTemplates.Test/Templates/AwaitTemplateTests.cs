using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class AwaitTemplateTests
{
    private readonly AwaitTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsAwait()
    {
        Assert.AreEqual("await", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleTask()
    {
        var result = _template.GetTransformedText("GetDataAsync()", "    ");

        Assert.AreEqual("await GetDataAsync()", result);
    }

    [TestMethod]
    public void GetTransformedText_Variable()
    {
        var result = _template.GetTransformedText("task", "    ");

        Assert.AreEqual("await task", result);
    }

    [TestMethod]
    public void GetTransformedText_ChainedCall()
    {
        var result = _template.GetTransformedText("service.FetchAsync()", "    ");

        Assert.AreEqual("await service.FetchAsync()", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("task", "    ");
        var resultB = _template.GetTransformedText("task", "        ");

        Assert.AreEqual(resultA, resultB, "AwaitTemplate output should not depend on indent");
    }
}
