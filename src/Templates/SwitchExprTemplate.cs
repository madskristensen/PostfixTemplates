using System;

namespace PostfixTemplates.Templates
{
    internal sealed class SwitchExprTemplate : PostfixTemplate
    {
        public override string Name => "switchexpr";

        public override string Description => "Produces switch expression";

        public override string Example => "expr switch { ... }";

        public override string Suffix => "switch expression";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"{expression} switch{Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    _ => default{Environment.NewLine}{indent}}}";
        }
    }
}
