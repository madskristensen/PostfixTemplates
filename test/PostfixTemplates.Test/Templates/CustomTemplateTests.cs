using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class CustomTemplateTests
{
    [TestMethod]
    public void Name_ReturnsDefinedName()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "Logger.Log({expr})" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual("log", template.Name);
    }

    [TestMethod]
    public void Description_ReturnsDefinedDescription()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Description = "Log it", Body = "Logger.Log({expr})" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual("Log it", template.Description);
    }

    [TestMethod]
    public void Description_FallsBackToName_WhenOmitted()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "Logger.Log({expr})" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual("log", template.Description);
    }

    [TestMethod]
    public void Suffix_ReturnsCustom()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "Logger.Log({expr})" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual("custom", template.Suffix);
    }

    [TestMethod]
    public void Suffix_ReturnsDefinedSuffix()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "Logger.Log({expr})", Suffix = "logging" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual("logging", template.Suffix);
    }

    [TestMethod]
    public void ApplicableTypes_DefaultsToAny()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "Logger.Log({expr})" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual(ExpressionType.Any, template.ApplicableTypes);
    }

    [TestMethod]
    public void ApplicableTypes_ParsesBoolean()
    {
        var definition = new CustomTemplateDefinition { Name = "check", Body = "Check({expr})", AppliesTo = "boolean" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual(ExpressionType.Boolean, template.ApplicableTypes);
    }

    [TestMethod]
    public void ApplicableTypes_IsCaseInsensitive()
    {
        var definition = new CustomTemplateDefinition { Name = "check", Body = "Check({expr})", AppliesTo = "Boolean" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual(ExpressionType.Boolean, template.ApplicableTypes);
    }

    [TestMethod]
    public void ApplicableTypes_FallsBackToAny_ForInvalidValue()
    {
        var definition = new CustomTemplateDefinition { Name = "test", Body = "Test({expr})", AppliesTo = "invalid" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual(ExpressionType.Any, template.ApplicableTypes);
    }

    [TestMethod]
    public void GetTransformedText_ReplacesExprPlaceholder()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "_logger.LogInformation({expr})" };
        var template = new CustomTemplate(definition);

        var result = template.GetTransformedText("myValue", "    ");

        Assert.AreEqual("_logger.LogInformation(myValue)", result);
    }

    [TestMethod]
    public void GetTransformedText_HandlesMultiplePlaceholders()
    {
        var definition = new CustomTemplateDefinition { Name = "test", Body = "Assert.That({expr}, Is.EqualTo({expr}))" };
        var template = new CustomTemplate(definition);

        var result = template.GetTransformedText("x", "    ");

        Assert.AreEqual("Assert.That(x, Is.EqualTo(x))", result);
    }

    [TestMethod]
    public void Example_ShowsBodyWithExprPlaceholderReplaced()
    {
        var definition = new CustomTemplateDefinition { Name = "log", Body = "_logger.LogInformation({expr})" };
        var template = new CustomTemplate(definition);

        Assert.AreEqual("_logger.LogInformation(expr)", template.Example);
    }

    [TestMethod]
    public void RequiresValueExpression_IsTrue()
    {
        var definition = new CustomTemplateDefinition { Name = "test", Body = "{expr}" };
        var template = new CustomTemplate(definition);

        Assert.IsTrue(template.RequiresValueExpression);
    }
}
