using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class NotWhitespaceTemplateTests
{
    private readonly NotWhitespaceTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsNotWhitespace()
    {
        Assert.AreEqual("notwhitespace", _template.Name);
    }

    [TestMethod]
    public void ApplicableTypes_IsString()
    {
        Assert.AreEqual(ExpressionType.String, _template.ApplicableTypes);
    }

    [TestMethod]
    public void GetTransformedText_GeneratesNegatedIsNullOrWhiteSpace()
    {
        var result = _template.GetTransformedText("myString", "    ");

        var expected = $"if (!string.IsNullOrWhiteSpace(myString)){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("s", "        ");

        var expected = $"if (!string.IsNullOrWhiteSpace(s)){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
