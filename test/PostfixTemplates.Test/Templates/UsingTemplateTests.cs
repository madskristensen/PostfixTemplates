using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class UsingTemplateTests
{
    private readonly UsingTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsUsing()
    {
        Assert.AreEqual("using", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleResource()
    {
        var result = _template.GetTransformedText("resource", "    ");

        var expected = $"using (resource){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCallReturningResource()
    {
        var result = _template.GetTransformedText("OpenStream()", "    ");

        var expected = $"using (OpenStream()){Environment.NewLine}    {{{Environment.NewLine}        {Environment.NewLine}    }}";
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetTransformedText_IndentAffectsOutput()
    {
        var result = _template.GetTransformedText("conn", "        ");

        var expected = $"using (conn){Environment.NewLine}        {{{Environment.NewLine}            {Environment.NewLine}        }}";
        Assert.AreEqual(expected, result);
    }
}
