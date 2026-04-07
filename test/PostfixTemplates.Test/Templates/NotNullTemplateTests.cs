using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class NotNullTemplateTests
{
    private readonly NotNullTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsNotNull()
    {
        Assert.AreEqual("notnull", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_GeneratesNotNullCheck()
    {
        var result = _template.GetTransformedText("myObject", "    ");

        var expected = $"if (myObject != null){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MemberAccess()
    {
        var result = _template.GetTransformedText("obj.Child", "    ");

        var expected = $"if (obj.Child != null){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("x", "        ");

        var expected = $"if (x != null){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
