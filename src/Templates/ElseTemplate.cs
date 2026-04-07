using System;

namespace PostfixTemplates.Templates
{
    internal sealed class ElseTemplate : PostfixTemplate
    {
        public override string Name => "else";

        public override string Description => "Checks boolean expression to be 'false'";

        public override string Example => "if (!expr) { }";

        public override string Suffix => "if not statement";

        public override ExpressionType ApplicableTypes => ExpressionType.Boolean;

        public override string GetTransformedText(string expression, string indent)
        {
            var needsParentheses = expression.Contains(" ") || expression.Contains("&&") || expression.Contains("||") || expression.Contains("==") || expression.Contains("!=") || expression.Contains("<") || expression.Contains(">");

            var negated = needsParentheses ? $"!({expression})" : $"!{expression}";
            return $"if ({negated}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
