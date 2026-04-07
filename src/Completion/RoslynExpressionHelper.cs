using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PostfixTemplates.Completion
{
    internal static class RoslynExpressionHelper
    {
        internal sealed class ExpressionResult(string text, int spanStart, int spanEnd, ExpressionSyntax expressionNode = null)
        {
            public string Text { get; } = text;
            public int SpanStart { get; } = spanStart;
            public int SpanEnd { get; } = spanEnd;

            /// <summary>
            /// The syntax node for the expression. Can be used to get type information
            /// from a semantic model.
            /// </summary>
            public ExpressionSyntax ExpressionNode { get; } = expressionNode;

            /// <summary>
            /// Whether the expression refers to a type rather than a value (e.g. <c>ExecutionContext.</c>).
            /// Set after resolving symbols via the semantic model.
            /// </summary>
            public bool IsTypeExpression { get; set; }
        }

        public static ExpressionResult FindExpressionBeforeDot(SyntaxTree tree, int dotPosition)
        {
            if (tree == null || dotPosition <= 0)
            {
                return null;
            }

            SyntaxNode root = tree.GetRoot();
            SyntaxToken token = root.FindToken(dotPosition - 1);

            if (token.IsKind(SyntaxKind.None))
            {
                return null;
            }

            SyntaxNode node = token.Parent;

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

                    ExpressionSyntax expression = memberAccess.Expression;
                    TextSpan span = expression.Span;
                    var text = expression.ToString();
                    return new ExpressionResult(text, span.Start, span.End, expression);
                }

                if (node is ExpressionSyntax exprSyntax && node.Parent is not MemberAccessExpressionSyntax)
                {
                    TextSpan exprSpan = exprSyntax.Span;
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

        /// <summary>
        /// Determines whether the given position is inside an async method or lambda.
        /// </summary>
        public static bool IsInAsyncContext(SyntaxTree tree, int position)
        {
            if (tree == null)
            {
                return false;
            }

            SyntaxNode root = tree.GetRoot();
            SyntaxNode node = root.FindToken(position).Parent;

            while (node != null)
            {
                if (node is MethodDeclarationSyntax method)
                {
                    return method.Modifiers.Any(SyntaxKind.AsyncKeyword);
                }

                if (node is LocalFunctionStatementSyntax localFunction)
                {
                    return localFunction.Modifiers.Any(SyntaxKind.AsyncKeyword);
                }

                if (node is ParenthesizedLambdaExpressionSyntax parenLambda)
                {
                    return parenLambda.AsyncKeyword.IsKind(SyntaxKind.AsyncKeyword);
                }

                if (node is SimpleLambdaExpressionSyntax simpleLambda)
                {
                    return simpleLambda.AsyncKeyword.IsKind(SyntaxKind.AsyncKeyword);
                }

                if (node is AnonymousMethodExpressionSyntax anonymousMethod)
                {
                    return anonymousMethod.AsyncKeyword.IsKind(SyntaxKind.AsyncKeyword);
                }

                node = node.Parent;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the given position is inside a method that returns an
        /// iterator type (IEnumerable, IEnumerable&lt;T&gt;, IEnumerator, IEnumerator&lt;T&gt;).
        /// </summary>
        public static bool IsInIteratorContext(SyntaxTree tree, int position, SemanticModel semanticModel)
        {
            if (tree == null)
            {
                return false;
            }

            SyntaxNode root = tree.GetRoot();
            SyntaxNode node = root.FindToken(position).Parent;

            while (node != null)
            {
                if (node is MethodDeclarationSyntax method)
                {
                    return IsIteratorReturnType(method.ReturnType, semanticModel);
                }

                if (node is LocalFunctionStatementSyntax localFunction)
                {
                    return IsIteratorReturnType(localFunction.ReturnType, semanticModel);
                }

                // Lambdas and anonymous methods cannot be iterators
                if (node is LambdaExpressionSyntax || node is AnonymousMethodExpressionSyntax)
                {
                    return false;
                }

                node = node.Parent;
            }

            return false;
        }

        private static bool IsIteratorReturnType(TypeSyntax returnType, SemanticModel semanticModel)
        {
            if (returnType == null || semanticModel == null)
            {
                return false;
            }

            ITypeSymbol typeSymbol = semanticModel.GetTypeInfo(returnType).Type;

            if (typeSymbol == null)
            {
                return false;
            }

            var name = typeSymbol.OriginalDefinition.ToDisplayString();
            return name == "System.Collections.IEnumerable"
                || name == "System.Collections.IEnumerator"
                || name == "System.Collections.Generic.IEnumerable<T>"
                || name == "System.Collections.Generic.IEnumerator<T>";
        }
    }
}
