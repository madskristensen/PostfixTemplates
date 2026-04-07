using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PostfixTemplateRegistryTests
{
    [TestMethod]
    public void All_ContainsExactly28Templates()
    {
        Assert.HasCount(28, PostfixTemplate.All);
    }

    [TestMethod]
    public void All_AllNamesAreUnique()
    {
        var names = PostfixTemplate.All.Select(t => t.Name).ToList();
        var uniqueNames = names.Distinct().ToList();

        Assert.HasCount(names.Count, uniqueNames, "Template names must be unique");
    }

    }
