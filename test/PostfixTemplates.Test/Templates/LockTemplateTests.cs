using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class LockTemplateTests
{
    private readonly LockTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsLock()
    {
        Assert.AreEqual("lock", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleObject()
    {
        var result = _template.GetTransformedText("_syncRoot", "    ");

        var expected = $"lock (_syncRoot){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_ThisReference()
    {
        var result = _template.GetTransformedText("this", "    ");

        var expected = $"lock (this){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("_lock", "        ");

        var expected = $"lock (_lock){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
