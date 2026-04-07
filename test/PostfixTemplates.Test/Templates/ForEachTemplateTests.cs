using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ForEachTemplateTests
{
    private readonly ForEachTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsForEach()
    {
        Assert.AreEqual("foreach", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleCollection()
    {
        var result = _template.GetTransformedText("collection", "    ");

        var expected = $"foreach (var item in collection){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCallReturningCollection()
    {
        var result = _template.GetTransformedText("GetItems()", "    ");

        var expected = $"foreach (var item in GetItems()){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("list", "        ");

        var expected = $"foreach (var item in list){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
