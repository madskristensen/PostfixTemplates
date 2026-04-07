using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ForRTemplateTests
{
    private readonly ForRTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsForR()
    {
        Assert.AreEqual("forr", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleCollection()
    {
        var result = _template.GetTransformedText("items", "    ");

        var expected = $"for (var i = items.Length - 1; i >= 0; i--){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCallReturningArray()
    {
        var result = _template.GetTransformedText("GetArray()", "    ");

        var expected = $"for (var i = GetArray().Length - 1; i >= 0; i--){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("list", "        ");

        var expected = $"for (var i = list.Length - 1; i >= 0; i--){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
