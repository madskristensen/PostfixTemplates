using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class NewTemplateTests
{
    private readonly NewTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsNew()
    {
        Assert.AreEqual("new", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleType()
    {
        var result = _template.GetTransformedText("MyClass", "    ");

        Assert.AreEqual("new MyClass()", result);
    }

    [TestMethod]
    public void GetTransformedText_GenericType()
    {
        var result = _template.GetTransformedText("List<int>", "    ");

        Assert.AreEqual("new List<int>()", result);
    }

    [TestMethod]
    public void GetTransformedText_QualifiedType()
    {
        var result = _template.GetTransformedText("System.Text.StringBuilder", "    ");

        Assert.AreEqual("new System.Text.StringBuilder()", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("MyClass", "    ");
        var resultB = _template.GetTransformedText("MyClass", "        ");

        Assert.AreEqual(resultA, resultB, "NewTemplate output should not depend on indent");
    }
}
