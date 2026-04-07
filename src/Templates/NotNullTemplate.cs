using System;

namespace PostfixTemplates.Templates
{
    internal sealed class NotNullTemplate : PostfixTemplate
    {
        public override string Name => "notnull";

        public override string Description => "Checks expression to be not null";

        public override string Example => "if (expr != null) { }";

        public override string Suffix => "not null check";

        public override ExpressionType ApplicableTypes => ExpressionType.Nullable;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"if ({expression} != null){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
