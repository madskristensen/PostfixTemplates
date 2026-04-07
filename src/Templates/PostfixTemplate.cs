using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace PostfixTemplates.Templates
{
    /// <summary>
    /// Specifies what kind of expression types a postfix template applies to.
    /// </summary>
    [Flags]
    internal enum ExpressionType
    {
        None = 0,
        Boolean = 1,
        Nullable = 2,
        Enumerable = 4,
        Exception = 8,
        Disposable = 16,
        Awaitable = 32,
        String = 64,
        ReferenceType = 128,

        /// <summary>
        /// Sentinel value indicating the template applies to any expression type.
        /// Not a combinable flag - checked explicitly via <see cref="Enum.HasFlag"/>.
        /// </summary>
        Any = int.MaxValue
    }

    internal abstract class PostfixTemplate
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Example { get; }

        /// <summary>
        /// Short suffix text displayed in gray after the completion item (e.g., "if statement").
        /// </summary>
        public abstract string Suffix { get; }

        /// <summary>
        /// Specifies what expression types this template applies to.
        /// </summary>
        public abstract ExpressionType ApplicableTypes { get; }

        /// <summary>
        /// When <see langword="true"/> (the default), the template only applies to value
        /// expressions and will be hidden when the expression resolves to a type symbol
        /// (e.g. <c>ExecutionContext.</c>).
        /// Override and return <see langword="false"/> for templates that are specifically
        /// designed to operate on a type name, such as <c>new</c>, <c>typeof</c>, or <c>inject</c>.
        /// </summary>
        public virtual bool RequiresValueExpression => true;

        /// <summary>
        /// When <see langword="true"/>, the template is only shown when the enclosing
        /// method or lambda has the <c>async</c> modifier (e.g. for <c>await</c>).
        /// </summary>
        public virtual bool RequiresAsyncContext => false;

        /// <summary>
        /// When <see langword="true"/>, the template is only shown when the enclosing
        /// method returns an iterator type such as <c>IEnumerable</c> or <c>IEnumerator</c>
        /// (e.g. for <c>yield return</c>).
        /// </summary>
        public virtual bool RequiresIteratorContext => false;

        /// <summary>
        /// Returns the placeholder text in the transformed output that should be
        /// selected after commit so the user can type over it immediately.
        /// Return <see langword="null"/> when no selection is needed.
        /// </summary>
        public virtual string SelectionPlaceholder => null;

        public abstract string GetTransformedText(string expression, string indent);

        /// <summary>
        /// Determines whether the given expression needs to be wrapped in parentheses
        /// before applying a negation operator.
        /// </summary>
        protected static bool NeedsParenthesesForNegation(string expression)
        {
            return expression.Contains(" ")
                || expression.Contains("&&")
                || expression.Contains("||")
                || expression.Contains("==")
                || expression.Contains("!=")
                || expression.Contains("<")
                || expression.Contains(">");
        }

        /// <summary>
        /// Determines if this template is applicable for the given expression type.
        /// </summary>
        public bool IsApplicableToType(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
            {
                // If we can't determine the type, show the template anyway
                return true;
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Any))
            {
                return true;
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Boolean))
            {
                if (typeSymbol.SpecialType == SpecialType.System_Boolean)
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Nullable))
            {
                // Reference types or nullable value types
                if (typeSymbol.IsReferenceType ||
                    typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Enumerable))
            {
                // Check if the type implements IEnumerable or has GetEnumerator
                if (IsEnumerable(typeSymbol))
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Exception))
            {
                // Check if the type derives from Exception
                if (IsException(typeSymbol))
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Disposable))
            {
                if (IsDisposable(typeSymbol))
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.Awaitable))
            {
                if (IsAwaitable(typeSymbol))
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.String))
            {
                if (typeSymbol.SpecialType == SpecialType.System_String)
                {
                    return true;
                }
            }

            if (ApplicableTypes.HasFlag(ExpressionType.ReferenceType))
            {
                if (typeSymbol.IsReferenceType)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsEnumerable(ITypeSymbol typeSymbol)
        {
            // Arrays are enumerable
            if (typeSymbol is IArrayTypeSymbol)
            {
                return true;
            }

            // Check for IEnumerable interface
            foreach (INamedTypeSymbol iface in typeSymbol.AllInterfaces)
            {
                if (iface.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T ||
                    iface.SpecialType == SpecialType.System_Collections_IEnumerable)
                {
                    return true;
                }
            }

            // Check if the type itself is IEnumerable
            if (typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T ||
                typeSymbol.SpecialType == SpecialType.System_Collections_IEnumerable)
            {
                return true;
            }

            return false;
        }

        private static bool IsException(ITypeSymbol typeSymbol)
        {
            ITypeSymbol current = typeSymbol;

            while (current != null)
            {
                if (current.Name == "Exception" &&
                    current.ContainingNamespace?.ToDisplayString() == "System")
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }

        private static bool IsDisposable(ITypeSymbol typeSymbol)
        {
            foreach (INamedTypeSymbol iface in typeSymbol.AllInterfaces)
            {
                if (iface.Name == "IDisposable" &&
                    iface.ContainingNamespace?.ToDisplayString() == "System")
                {
                    return true;
                }
            }

            if (typeSymbol is INamedTypeSymbol namedType &&
                namedType.Name == "IDisposable" &&
                namedType.ContainingNamespace?.ToDisplayString() == "System")
            {
                return true;
            }

            return false;
        }

        private static bool IsAwaitable(ITypeSymbol typeSymbol)
        {
            // A type is awaitable if it has a GetAwaiter() method accessible on it.
            // This covers Task, Task<T>, ValueTask, ValueTask<T>, and any custom awaitables.
            foreach (ISymbol member in typeSymbol.GetMembers("GetAwaiter"))
            {
                if (member is IMethodSymbol method && method.Parameters.Length == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static IReadOnlyList<PostfixTemplate> All { get; } =
        [
            new ArgTemplate(),
            new AsTemplate(),
            new AssertTemplate(),
            new AwaitTemplate(),
            new CastTemplate(),
            new ConditionalTemplate(),
            new DiscardTemplate(),
            new ElseTemplate(),
            new FieldTemplate(),
            new ForTemplate(),
            new ForRTemplate(),
            new ForEachTemplate(),
            new IfTemplate(),
            new InjectTemplate(),
            new IsTemplate(),
            new LambdaTemplate(),
            new LockTemplate(),
            new NameofTemplate(),
            new NewTemplate(),
            new NotTemplate(),
            new NotEmptyTemplate(),
            new NotWhitespaceTemplate(),
            new NullTemplate(),
            new NotNullTemplate(),
            new ParTemplate(),
            new ParseTemplate(),
            new PropTemplate(),
            new ReturnTemplate(),
            new SwitchTemplate(),
            new SwitchExprTemplate(),
            new ThrowTemplate(),
            new ThrowIfNullTemplate(),
            new ToTemplate(),
            new TryCatchTemplate(),
            new TryParseTemplate(),
            new TypeofTemplate(),
            new UsingTemplate(),
            new VarTemplate(),
            new WhileTemplate(),
            new WriteLineTemplate(),
            new YieldTemplate()
        ];

        /// <summary>
        /// O(1) lookup by template name for use in commit and description paths.
        /// </summary>
        public static IReadOnlyDictionary<string, PostfixTemplate> ByName { get; } =
            All.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
    }
}
