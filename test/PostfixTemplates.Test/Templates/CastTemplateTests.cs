using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class CastTemplateTests
{
    private readonly CastTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsCast()
    {
        Assert.AreEqual("cast", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleVariable()
    {
        var result = _template.GetTransformedText("obj", "    ");

        Assert.AreEqual("((SomeType) obj)", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall()
    {
        var result = _template.GetTransformedText("GetValue()", "    ");

        Assert.AreEqual("((SomeType) GetValue())", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("obj", "    ");
        var resultB = _template.GetTransformedText("obj", "        ");

        Assert.AreEqual(resultA, resultB, "CastTemplate output should not depend on indent");
    }
}
