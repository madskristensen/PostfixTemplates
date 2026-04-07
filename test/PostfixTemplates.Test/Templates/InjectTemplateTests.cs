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
    public void GetTransformedText_ParameterNameIsAlwaysDependency()
    {
        // The parameter name is always "dependency" regardless of the type name convention.
        var resultA = _template.GetTransformedText("IMyServiceImpl", "    ");
        var resultB = _template.GetTransformedText("MyRepository", "    ");
        var resultC = _template.GetTransformedText("ILogger", "    ");

        Assert.AreEqual("(IMyServiceImpl dependency)", resultA);
        Assert.AreEqual("(MyRepository dependency)", resultB);
        Assert.AreEqual("(ILogger dependency)", resultC);
    }
}
