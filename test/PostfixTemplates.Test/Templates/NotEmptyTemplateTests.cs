using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class NotEmptyTemplateTests
{
    private readonly NotEmptyTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsNotEmpty()
    {
        Assert.AreEqual("notempty", _template.Name);
    }

    [TestMethod]
    public void ApplicableTypes_IsString()
    {
        Assert.AreEqual(ExpressionType.String, _template.ApplicableTypes);
    }

    [TestMethod]
    public void GetTransformedText_GeneratesNegatedIsNullOrEmpty()
    {
        var result = _template.GetTransformedText("myString", "    ");

        var expected = $"if (!string.IsNullOrEmpty(myString)){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("s", "        ");

        var expected = $"if (!string.IsNullOrEmpty(s)){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
