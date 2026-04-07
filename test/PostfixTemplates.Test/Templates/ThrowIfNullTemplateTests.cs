using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class ThrowIfNullTemplateTests
{
    private readonly ThrowIfNullTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsThrowIfNull()
    {
        Assert.AreEqual("throwifnull", _template.Name);
    }

    [TestMethod]
    public void ApplicableTypes_IsNullable()
    {
        Assert.AreEqual(ExpressionType.Nullable, _template.ApplicableTypes);
    }

    [TestMethod]
    public void GetTransformedText_GeneratesThrowIfNull()
    {
        var result = _template.GetTransformedText("param", "    ");

        Assert.AreEqual("ArgumentNullException.ThrowIfNull(param)", result);
    }
}
