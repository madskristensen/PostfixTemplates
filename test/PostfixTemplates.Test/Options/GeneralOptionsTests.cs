using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PostfixTemplates.Test.Options;

[TestClass]
public class GeneralOptionsTests
{
    [TestMethod]
    public void IsTemplateEnabled_AllEnabledByDefault()
    {
        var settings = new General();

        var templateNames = new[] { "if", "else", "var", "not", "null", "notnull", "return", "foreach", "while", "throw" };
        foreach (var name in templateNames)
        {
            Assert.IsTrue(settings.IsTemplateEnabled(name), $"Template '{name}' should be enabled by default");
        }
    }

    [TestMethod]
    public void IsTemplateEnabled_ReturnsFalseForUnknownTemplate()
    {
        var settings = new General();

        Assert.IsFalse(settings.IsTemplateEnabled("unknown"));
    }

    [TestMethod]
    public void IsTemplateEnabled_ReturnsFalseForEmptyString()
    {
        var settings = new General();

        Assert.IsFalse(settings.IsTemplateEnabled(""));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledIf()
    {
        var settings = new General { EnableIf = false };

        Assert.IsFalse(settings.IsTemplateEnabled("if"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledElse()
    {
        var settings = new General { EnableElse = false };

        Assert.IsFalse(settings.IsTemplateEnabled("else"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledVar()
    {
        var settings = new General { EnableVar = false };

        Assert.IsFalse(settings.IsTemplateEnabled("var"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledNot()
    {
        var settings = new General { EnableNot = false };

        Assert.IsFalse(settings.IsTemplateEnabled("not"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledNull()
    {
        var settings = new General { EnableNull = false };

        Assert.IsFalse(settings.IsTemplateEnabled("null"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledNotNull()
    {
        var settings = new General { EnableNotNull = false };

        Assert.IsFalse(settings.IsTemplateEnabled("notnull"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledReturn()
    {
        var settings = new General { EnableReturn = false };

        Assert.IsFalse(settings.IsTemplateEnabled("return"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledForEach()
    {
        var settings = new General { EnableForEach = false };

        Assert.IsFalse(settings.IsTemplateEnabled("foreach"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledWhile()
    {
        var settings = new General { EnableWhile = false };

        Assert.IsFalse(settings.IsTemplateEnabled("while"));
    }

    [TestMethod]
    public void IsTemplateEnabled_RespectsDisabledThrow()
    {
        var settings = new General { EnableThrow = false };

        Assert.IsFalse(settings.IsTemplateEnabled("throw"));
    }

    [TestMethod]
    public void IsTemplateEnabled_DisablingOneDoesNotAffectOthers()
    {
        var settings = new General { EnableIf = false };

        Assert.IsFalse(settings.IsTemplateEnabled("if"));
        Assert.IsTrue(settings.IsTemplateEnabled("else"));
        Assert.IsTrue(settings.IsTemplateEnabled("var"));
    }
}
