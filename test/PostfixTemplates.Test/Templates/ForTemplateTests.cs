using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ForTemplateTests
{
    private readonly ForTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsFor()
    {
        Assert.AreEqual("for", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleCollection()
    {
        var result = _template.GetTransformedText("items", "    ");

        var expected = $"for (var i = 0; i < items.Length; i++){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCallReturningArray()
    {
        var result = _template.GetTransformedText("GetArray()", "    ");

        var expected = $"for (var i = 0; i < GetArray().Length; i++){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("list", "        ");

        var expected = $"for (var i = 0; i < list.Length; i++){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
