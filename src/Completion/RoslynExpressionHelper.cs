using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PostfixTemplates.Completion
{
    internal static class RoslynExpressionHelper
    {
        internal sealed class ExpressionResult
        {
            public ExpressionResult(string text, int spanStart, int spanEnd, ExpressionSyntax expressionNode = null)
            {
                Text = text;
                SpanStart = spanStart;
                SpanEnd = spanEnd;
                ExpressionNode = expressionNode;
            }

            public string Text { get; }
            public int SpanStart { get; }
            public int SpanEnd { get; }

            /// <summary>
            /// The syntax node for the expression. Can be used to get type information
            /// from a semantic model.
            /// </summary>
            public ExpressionSyntax ExpressionNode { get; }
        }

        public static ExpressionResult FindExpressionBeforeDot(SyntaxTree tree, int dotPosition)
        {
            if (tree == null || dotPosition <= 0)
            {
                return null;
            }

            var root = tree.GetRoot();
            var token = root.FindToken(dotPosition - 1);

            if (token.IsKind(SyntaxKind.None))
            {
                return null;
            }

            var node = token.Parent;

            while (node != null)
            {
                if (node is MemberAccessExpressionSyntax memberAccess)
                {
                    // If this MemberAccessExpression's parent is also a MemberAccessExpression
                    // and this node is the Expression (left side) of that parent, continue
                    // walking up to find the outermost member access containing the trailing dot.
                    if (node.Parent is MemberAccessExpressionSyntax parentMemberAccess
                        && parentMemberAccess.Expression == node)
                    {
                        node = node.Parent;
                        continue;
                    }

                    var expression = memberAccess.Expression;
                    var span = expression.Span;
                    var text = expression.ToString();
                    return new ExpressionResult(text, span.Start, span.End, expression);
                }

                if (node is ExpressionSyntax exprSyntax && !(node.Parent is MemberAccessExpressionSyntax))
                {
                    var exprSpan = exprSyntax.Span;
                    var exprText = exprSyntax.ToString();

                    if (exprSpan.End == dotPosition - 1 || exprSpan.End == dotPosition)
                    {
                        return new ExpressionResult(exprText, exprSpan.Start, exprSpan.End, exprSyntax);
                    }
                }

                node = node.Parent;
            }

            return null;
        }
    }
}
