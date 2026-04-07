using System;

namespace PostfixTemplates.Templates
{
    internal sealed class NotEmptyTemplate : PostfixTemplate
    {
        public override string Name => "notempty";

        public override string Description => "Checks if string is not null or empty";

        public override string Example => "if (!string.IsNullOrEmpty(expr)) { }";

        public override string Suffix => "null or empty check";

        public override ExpressionType ApplicableTypes => ExpressionType.String;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"if (!string.IsNullOrEmpty({expression})){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
