using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class SwitchTemplateTests
{
    private readonly SwitchTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsSwitch()
    {
        Assert.AreEqual("switch", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleVariable()
    {
        var result = _template.GetTransformedText("status", "    ");

        var expected = $"switch (status){Environment.NewLine}    {{{Environment.NewLine}        case value:{Environment.NewLine}            break;{Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("GetStatus()", "    ");

        var expected = $"switch (GetStatus()){Environment.NewLine}    {{{Environment.NewLine}        case value:{Environment.NewLine}            break;{Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("value", "        ");

        var expected = $"switch (value){Environment.NewLine}        {{{Environment.NewLine}            case value:{Environment.NewLine}                break;{Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
