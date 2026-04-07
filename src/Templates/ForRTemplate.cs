using System;

namespace PostfixTemplates.Templates
{
    internal sealed class ForRTemplate : PostfixTemplate
    {
        public override string Name => "forr";

        public override string Description => "Iterates over collection in reverse with index";

        public override string Example => "for (var i = xs.Length-1; i >= 0; i--)";

        public override string Suffix => "reverse for loop";

        public override ExpressionType ApplicableTypes => ExpressionType.Enumerable;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"for (var i = {expression}.Length - 1; i >= 0; i--){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
