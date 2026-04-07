using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PostfixTemplateRegistryTests
{
    [TestMethod]
    public void All_ContainsExactly29Templates()
    {
        Assert.HasCount(29, PostfixTemplate.All);
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
        foreach (PostfixTemplate? template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Name), $"Template name cannot be null or empty");
        }
    }

    [TestMethod]
    public void All_NoTemplateHasNullOrEmptyDescription()
    {
        foreach (PostfixTemplate? template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Description), $"Template {template.Name} has null or empty description");
        }
    }

    [TestMethod]
    public void All_NoTemplateHasNullOrEmptyExample()
    {
        foreach (PostfixTemplate? template in PostfixTemplate.All)
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

    [TestMethod]
    public void All_ContainsArgTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "arg"));
    }

    [TestMethod]
    public void All_ContainsAwaitTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "await"));
    }

    [TestMethod]
    public void All_ContainsCastTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "cast"));
    }

    [TestMethod]
    public void All_ContainsFieldTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "field"));
    }

    [TestMethod]
    public void All_ContainsForTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "for"));
    }

    [TestMethod]
    public void All_ContainsForRTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "forr"));
    }

    [TestMethod]
    public void All_ContainsInjectTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "inject"));
    }

    [TestMethod]
    public void All_ContainsLockTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "lock"));
    }

    [TestMethod]
    public void All_ContainsNewTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "new"));
    }

    [TestMethod]
    public void All_ContainsParTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "par"));
    }

    [TestMethod]
    public void All_ContainsParseTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "parse"));
    }

    [TestMethod]
    public void All_ContainsPropTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "prop"));
    }

    [TestMethod]
    public void All_ContainsSelTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "sel"));
    }

    [TestMethod]
    public void All_ContainsSwitchTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "switch"));
    }

    [TestMethod]
    public void All_ContainsToTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "to"));
    }

    [TestMethod]
    public void All_ContainsTryParseTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "tryparse"));
    }

    [TestMethod]
    public void All_ContainsTypeofTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "typeof"));
    }

    [TestMethod]
    public void All_ContainsUsingTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "using"));
    }

    [TestMethod]
    public void All_ContainsYieldTemplate()
    {
        Assert.IsTrue(PostfixTemplate.All.Any(t => t.Name == "yield"));
    }
}
