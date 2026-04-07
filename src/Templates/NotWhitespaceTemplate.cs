using System;

namespace PostfixTemplates.Templates
{
    internal sealed class NotWhitespaceTemplate : PostfixTemplate
    {
        public override string Name => "notwhitespace";

        public override string Description => "Checks if string is not null or whitespace";

        public override string Example => "if (!string.IsNullOrWhiteSpace(expr)) { }";

        public override string Suffix => "null or whitespace check";

        public override ExpressionType ApplicableTypes => ExpressionType.String;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"if (!string.IsNullOrWhiteSpace({expression})){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
