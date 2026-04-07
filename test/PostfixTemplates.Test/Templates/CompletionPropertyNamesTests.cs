using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Completion;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class CompletionPropertyNamesTests
{
    [TestMethod]
    public void PostfixTemplate_HasExpectedValue()
    {
        Assert.AreEqual("PostfixTemplate", CompletionPropertyNames.PostfixTemplate);
    }

    [TestMethod]
    public void ExpressionText_HasExpectedValue()
    {
        Assert.AreEqual("ExpressionText", CompletionPropertyNames.ExpressionText);
    }

    [TestMethod]
    public void ExpressionStart_HasExpectedValue()
    {
        Assert.AreEqual("ExpressionStart", CompletionPropertyNames.ExpressionStart);
    }

    [TestMethod]
    public void DotPosition_HasExpectedValue()
    {
        Assert.AreEqual("DotPosition", CompletionPropertyNames.DotPosition);
    }
}
