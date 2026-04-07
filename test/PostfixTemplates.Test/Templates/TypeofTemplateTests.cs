using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class TypeofTemplateTests
{
    private readonly TypeofTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsTypeof()
    {
        Assert.AreEqual("typeof", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleType()
    {
        var result = _template.GetTransformedText("MyClass", "    ");

        Assert.AreEqual("typeof(MyClass)", result);
    }

    [TestMethod]
    public void GetTransformedText_GenericType()
    {
        var result = _template.GetTransformedText("List<int>", "    ");

        Assert.AreEqual("typeof(List<int>)", result);
    }

    [TestMethod]
    public void GetTransformedText_PrimitiveType()
    {
        var result = _template.GetTransformedText("string", "    ");

        Assert.AreEqual("typeof(string)", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("MyType", "    ");
        var resultB = _template.GetTransformedText("MyType", "        ");

        Assert.AreEqual(resultA, resultB, "TypeofTemplate output should not depend on indent");
    }
}
