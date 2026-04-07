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
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("myVar", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(5, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_MethodCallChain()
    {
        var code = "obj.Method().";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.LastIndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("obj.Method()", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(12, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_ParenthesizedExpression()
    {
        var code = "(a + b).";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("(a + b)", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(7, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_Constructor()
    {
        var code = "new List<int>().";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("new List<int>()", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(15, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_Indexer()
    {
        var code = "arr[0].";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("arr[0]", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(6, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_NullWhenNegativePosition()
    {
        var code = "myVar.";
        var tree = CSharpSyntaxTree.ParseText(code);

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, -1);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_NullWhenNullTree()
    {
        var result = RoslynExpressionHelper.FindExpressionBeforeDot(null, 5);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_MemberAccess()
    {
        var code = "obj.Property.";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.LastIndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("obj.Property", result.Text);
        Assert.AreEqual(0, result.SpanStart);
        Assert.AreEqual(12, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_InsideMethodBody()
    {
        var code = "class C { void M() { myVar. } }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("myVar", result.Text);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_ChainInsideMethodBody()
    {
        var code = "class C { void M() { list.Where(x => x > 0). } }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.LastIndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("list.Where(x => x > 0)", result.Text);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_SpanValuesMatchTextPosition()
    {
        var code = "class C { void M() { value. } }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var dotPosition = code.IndexOf('.');

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition);

        Assert.IsNotNull(result);
        Assert.AreEqual("value", result.Text);
        Assert.AreEqual(code.IndexOf("value"), result.SpanStart);
        Assert.AreEqual(code.IndexOf("value") + "value".Length, result.SpanEnd);
    }

    [TestMethod]
    public void FindExpressionBeforeDot_NullWhenPositionZero()
    {
        var code = "myVar.";
        var tree = CSharpSyntaxTree.ParseText(code);

        var result = RoslynExpressionHelper.FindExpressionBeforeDot(tree, 0);

        Assert.IsNull(result);
    }
}
