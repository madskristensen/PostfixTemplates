using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class CustomTemplateLoaderTests
{
    [TestMethod]
    public void ParseJson_ReturnsEmpty_ForNull()
    {
        var result = CustomTemplateLoader.ParseJson(null);

        Assert.HasCount(0, result);
    }

    [TestMethod]
    public void ParseJson_ReturnsEmpty_ForEmptyString()
    {
        var result = CustomTemplateLoader.ParseJson("");

        Assert.HasCount(0, result);
    }

    [TestMethod]
    public void ParseJson_ReturnsEmpty_ForInvalidJson()
    {
        var result = CustomTemplateLoader.ParseJson("not json");

        Assert.HasCount(0, result);
    }

    [TestMethod]
    public void ParseJson_ReturnsEmpty_ForEmptyTemplatesArray()
    {
        var json = @"{ ""templates"": [] }";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(0, result);
    }

    [TestMethod]
    public void ParseJson_ParsesSingleTemplate()
    {
        var json = @"{
  ""templates"": [
    {
      ""name"": ""log"",
      ""description"": ""Log expression"",
      ""body"": ""_logger.LogInformation({expr})"",
      ""appliesTo"": ""any""
    }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(1, result);
        Assert.AreEqual("log", result[0].Name);
        Assert.AreEqual("Log expression", result[0].Description);
        Assert.AreEqual(ExpressionType.Any, result[0].ApplicableTypes);
    }

    [TestMethod]
    public void ParseJson_ParsesMultipleTemplates()
    {
        var json = @"{
  ""templates"": [
    { ""name"": ""log"", ""body"": ""Log({expr})"" },
    { ""name"": ""debug"", ""body"": ""Debug({expr})"" }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(2, result);
        Assert.AreEqual("log", result[0].Name);
        Assert.AreEqual("debug", result[1].Name);
    }

    [TestMethod]
    public void ParseJson_SkipsEntries_WithMissingName()
    {
        var json = @"{
  ""templates"": [
    { ""body"": ""Log({expr})"" },
    { ""name"": ""debug"", ""body"": ""Debug({expr})"" }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(1, result);
        Assert.AreEqual("debug", result[0].Name);
    }

    [TestMethod]
    public void ParseJson_SkipsEntries_WithMissingBody()
    {
        var json = @"{
  ""templates"": [
    { ""name"": ""log"" },
    { ""name"": ""debug"", ""body"": ""Debug({expr})"" }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(1, result);
        Assert.AreEqual("debug", result[0].Name);
    }

    [TestMethod]
    public void ParseJson_HandlesAppliesTo_Boolean()
    {
        var json = @"{
  ""templates"": [
    { ""name"": ""check"", ""body"": ""Check({expr})"", ""appliesTo"": ""boolean"" }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(1, result);
        Assert.AreEqual(ExpressionType.Boolean, result[0].ApplicableTypes);
    }

    [TestMethod]
    public void ParseJson_DefaultsToAny_WhenAppliesToOmitted()
    {
        var json = @"{
  ""templates"": [
    { ""name"": ""log"", ""body"": ""Log({expr})"" }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(1, result);
        Assert.AreEqual(ExpressionType.Any, result[0].ApplicableTypes);
    }

    [TestMethod]
    public void ParseJson_IgnoresExtraJsonProperties()
    {
        var json = @"{
  ""$schema"": ""https://example.com/schema.json"",
  ""templates"": [
    { ""name"": ""log"", ""body"": ""Log({expr})"", ""extra"": true }
  ]
}";

        var result = CustomTemplateLoader.ParseJson(json);

        Assert.HasCount(1, result);
        Assert.AreEqual("log", result[0].Name);
    }
}
