using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;
using System.Linq;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PostfixTemplateRegistryTests
{
    [TestMethod]
    public void All_ContainsExactly10Templates()
    {
        Assert.HasCount(10, PostfixTemplate.All);
    }

    [TestMethod]
    public void All_AllNamesAreUnique()
    {
        var names = PostfixTemplate.All.Select(t => t.Name).ToList();
        var uniqueNames = names.Distinct().ToList();

        Assert.HasCount(names.Count, uniqueNames, "Template names must be unique");
    }

    [TestMethod]
    public void All_NoTemplateHasNullOrEmptyName()
    {
        foreach (var template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Name), $"Template name cannot be null or empty");
        }
    }

    [TestMethod]
    public void All_NoTemplateHasNullOrEmptyDescription()
    {
        foreach (var template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Description), $"Template {template.Name} has null or empty description");
        }
    }

    [TestMethod]
    public void All_NoTemplateHasNullOrEmptyExample()
    {
        foreach (var template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Example), $"Template {template.Name} has null or empty example");
        }
    }

    [TestMethod]
    public void All_ContainsIfTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "if"));
    }

    [TestMethod]
    public void All_ContainsElseTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "else"));
    }

    [TestMethod]
    public void All_ContainsVarTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "var"));
    }

    [TestMethod]
    public void All_ContainsNotTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "not"));
    }

    [TestMethod]
    public void All_ContainsNullTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "null"));
    }

    [TestMethod]
    public void All_ContainsNotNullTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "notnull"));
    }

    [TestMethod]
    public void All_ContainsReturnTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "return"));
    }

    [TestMethod]
    public void All_ContainsForEachTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "foreach"));
    }

    [TestMethod]
    public void All_ContainsWhileTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "while"));
    }

    [TestMethod]
    public void All_ContainsThrowTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "throw"));
    }
}
