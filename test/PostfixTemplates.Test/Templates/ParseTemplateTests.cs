using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ParseTemplateTests
{
    private readonly ParseTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsParse()
    {
        Assert.AreEqual("parse", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleString()
    {
        var result = _template.GetTransformedText("input", "    ");

        Assert.AreEqual("int.Parse(input)", result);
    }

    [TestMethod]
    public void GetTransformedText_Variable()
    {
        var result = _template.GetTransformedText("text", "    ");

        Assert.AreEqual("int.Parse(text)", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("reader.ReadLine()", "    ");

        Assert.AreEqual("int.Parse(reader.ReadLine())", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("s", "    ");
        var resultB = _template.GetTransformedText("s", "        ");

        Assert.AreEqual(resultA, resultB, "ParseTemplate output should not depend on indent");
    }
}
