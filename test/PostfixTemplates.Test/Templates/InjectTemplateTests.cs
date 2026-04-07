using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class InjectTemplateTests
{
    private readonly InjectTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsInject()
    {
        Assert.AreEqual("inject", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_Interface()
    {
        var result = _template.GetTransformedText("IMyService", "    ");

        Assert.AreEqual("(IMyService dependency)", result);
    }

    [TestMethod]
    public void GetTransformedText_ConcreteType()
    {
        var result = _template.GetTransformedText("MyRepository", "    ");

        Assert.AreEqual("(MyRepository dependency)", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("IService", "    ");
        var resultB = _template.GetTransformedText("IService", "        ");

        Assert.AreEqual(resultA, resultB, "InjectTemplate output should not depend on indent");
    }
}
