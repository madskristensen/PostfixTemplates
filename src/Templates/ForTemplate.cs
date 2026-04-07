using System;

namespace PostfixTemplates.Templates
{
    internal sealed class ForTemplate : PostfixTemplate
    {
        public override string Name => "for";

        public override string Description => "Iterates over collection with index";

        public override string Example => "for (var i = 0; i < xs.Length; i++)";

        public override string Suffix => "for loop";

        public override ExpressionType ApplicableTypes => ExpressionType.Enumerable;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"for (var i = 0; i < {expression}.Length; i++){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
