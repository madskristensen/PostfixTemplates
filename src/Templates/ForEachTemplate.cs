using System;

namespace PostfixTemplates.Templates
{
    internal sealed class ForEachTemplate : PostfixTemplate
    {
        public override string Name => "foreach";

        public override string Description => "Iterates over enumerable collection";

        public override string Example => "foreach (var item in expr) { }";

        public override string Suffix => "foreach loop";

        public override ExpressionType ApplicableTypes => ExpressionType.Enumerable;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"foreach (var item in {expression}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
