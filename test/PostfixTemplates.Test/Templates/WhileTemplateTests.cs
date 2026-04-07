using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class WhileTemplateTests
{
    private readonly WhileTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsWhile()
    {
        Assert.AreEqual("while", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleCondition()
    {
        var result = _template.GetTransformedText("condition", "    ");

        var expected = $"while (condition){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("reader.Read()", "    ");

        var expected = $"while (reader.Read()){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("running", "        ");

        var expected = $"while (running){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
