using System.Collections.Generic;
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
        Any = 16
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

        public abstract string GetTransformedText(string expression, string indent);

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

        public static IReadOnlyList<PostfixTemplate> All { get; } =
        [
            new ArgTemplate(),
            new AwaitTemplate(),
            new CastTemplate(),
            new ElseTemplate(),
            new FieldTemplate(),
            new ForTemplate(),
            new ForRTemplate(),
            new ForEachTemplate(),
            new IfTemplate(),
            new InjectTemplate(),
            new LockTemplate(),
            new NewTemplate(),
            new NotTemplate(),
            new NullTemplate(),
            new NotNullTemplate(),
            new ParTemplate(),
            new ParseTemplate(),
            new PropTemplate(),
            new ReturnTemplate(),
            new SelTemplate(),
            new SwitchTemplate(),
            new ThrowTemplate(),
            new ToTemplate(),
            new TryParseTemplate(),
            new TypeofTemplate(),
            new UsingTemplate(),
            new VarTemplate(),
            new WhileTemplate(),
            new YieldTemplate()
        ];
    }
}
