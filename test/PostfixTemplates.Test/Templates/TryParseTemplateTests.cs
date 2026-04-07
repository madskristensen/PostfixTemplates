using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class TryParseTemplateTests
{
    private readonly TryParseTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsTryParse()
    {
        Assert.AreEqual("tryparse", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleString()
    {
        var result = _template.GetTransformedText("input", "    ");

        Assert.AreEqual("int.TryParse(input, out var value)", result);
    }

    [TestMethod]
    public void GetTransformedText_Variable()
    {
        var result = _template.GetTransformedText("text", "    ");

        Assert.AreEqual("int.TryParse(text, out var value)", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("reader.ReadLine()", "    ");

        Assert.AreEqual("int.TryParse(reader.ReadLine(), out var value)", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("s", "    ");
        var resultB = _template.GetTransformedText("s", "        ");

        Assert.AreEqual(resultA, resultB, "TryParseTemplate output should not depend on indent");
    }
}
