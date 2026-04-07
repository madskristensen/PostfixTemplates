using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostfixTemplates.Completion;

namespace PostfixTemplates.Test;

[TestClass]
public class RoslynExpressionHelperTests
{
    [TestMethod]
    public void FindExpressionBeforeDot_SimpleIdentifier()
    {
        var code = "myVar.";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("myVar", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(5, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_MethodCallChain()
    {
        var code = "obj.Method().";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.LastIndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("obj.Method()", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(12, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_ParenthesizedExpression()
    {
        var code = "(a + b).";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("(a + b)", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(7, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_Constructor()
    {
        var code = "new List<int>().";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("new List<int>()", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(15, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_Indexer()
    {
        var code = "arr[0].";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("arr[0]", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(6, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_NullWhenNegativePosition()
    {
        var code = "myVar.";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, -1);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_NullWhenNullTree()
    {
        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(null, 5);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_MemberAccess()
    {
        var code = "obj.Property.";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.LastIndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("obj.Property", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(12, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_InsideMethodBody()
    {
        var code = "class C { void M() { myVar. } }";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("myVar", result.Text);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_ChainInsideMethodBody()
    {
        var code = "class C { void M() { list.Where(x => x > 0). } }";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.LastIndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("list.Where(x => x > 0)", result.Text);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_SpanValuesMatchTextPosition()
    {
        var code = "class C { void M() { value. } }";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("value", result.Text);
        Assert.AreEqual(code.IndexOf("value"), result.SpanStart);
        Assert.AreEqual(code.IndexOf("value") + "value".Length, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_NullWhenPositionZero()
    {
        var code = "myVar.";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

        RoslynExpressionHelper.ExpressionResult result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, 0);

        Assert.IsNull(result);
    }
}
