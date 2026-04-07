using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ThrowTemplateTests
{
    private readonly ThrowTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsThrow()
    {
        Assert.AreEqual("throw", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_NewException()
    {
        var result = _template.GetTransformedText("new Exception()", "    ");

        Assert.AreEqual("throw new Exception();", result);
    }

    [TestMethod]
    public void GetTransformedText_ExceptionWithMessage()
    {
        var result = _template.GetTransformedText("new ArgumentException(\"bad\")", "    ");

        Assert.AreEqual("throw new ArgumentException(\"bad\");", result);
    }

    [TestMethod]
    public void GetTransformedText_Variable()
    {
        var result = _template.GetTransformedText("ex", "    ");

        Assert.AreEqual("throw ex;", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("ex", "    ");
        var resultB = _template.GetTransformedText("ex", "        ");

        Assert.AreEqual(resultA, resultB, "ThrowTemplate output should not depend on indent");
    }
}
