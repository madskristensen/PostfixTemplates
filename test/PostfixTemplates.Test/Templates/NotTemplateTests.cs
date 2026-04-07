using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class NotTemplateTests
{
    private readonly NotTemplate _template = new();

    [TestMethod]
    public void Name_ReturnsNot()
    {
        Assert.AreEqual("not", _template.Name);
    }

    [TestMethod]
    public void GetTransformedText_SimpleIdentifier_NoParentheses()
    {
        var result = _template.GetTransformedText("isReady", "    ");

        Assert.AreEqual("!isReady", result);
    }

    [TestMethod]
    public void GetTransformedText_MemberAccess_NoParentheses()
    {
        var result = _template.GetTransformedText("obj.IsValid", "    ");

        Assert.AreEqual("!obj.IsValid", result);
    }

    [TestMethod]
    public void GetTransformedText_MethodCall_NoParentheses()
    {
        var result = _template.GetTransformedText("Check()", "    ");

        Assert.AreEqual("!Check()", result);
    }

    [TestMethod]
    public void GetTransformedText_LogicalAnd_WithParentheses()
    {
        var result = _template.GetTransformedText("a && b", "    ");

        Assert.AreEqual("!(a && b)", result);
    }

    [TestMethod]
    public void GetTransformedText_LogicalOr_WithParentheses()
    {
        var result = _template.GetTransformedText("a || b", "    ");

        Assert.AreEqual("!(a || b)", result);
    }

    [TestMethod]
    public void GetTransformedText_Equality_WithParentheses()
    {
        var result = _template.GetTransformedText("x == 5", "    ");

        Assert.AreEqual("!(x == 5)", result);
    }

    [TestMethod]
    public void GetTransformedText_Inequality_WithParentheses()
    {
        var result = _template.GetTransformedText("x != null", "    ");

        Assert.AreEqual("!(x != null)", result);
    }

    [TestMethod]
    public void GetTransformedText_ComparisonOperator_WithParentheses()
    {
        var result = _template.GetTransformedText("x > 5", "    ");

        Assert.AreEqual("!(x > 5)", result);
    }

    [TestMethod]
    public void GetTransformedText_GenericTypeName_WithParentheses()
    {
        // The '<' and '>' in a generic type name trigger the parenthesization heuristic,
        // so "List<int>" is wrapped even though it is not a comparison expression.
        var result = _template.GetTransformedText("List<int>", "    ");

        Assert.AreEqual("!(List<int>)", result);
    }

    [TestMethod]
    public void GetTransformedText_IndentIsIgnored()
    {
        var resultA = _template.GetTransformedText("flag", "    ");
        var resultB = _template.GetTransformedText("flag", "        ");

        Assert.AreEqual(resultA, resultB, "NotTemplate output should not depend on indent");
    }
}
