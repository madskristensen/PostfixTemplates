using System;

namespace PostfixTemplates.Templates
{
    internal sealed class SwitchTemplate : PostfixTemplate
    {
        public override string Name => "switch";

        public override string Description => "Produces switch statement";

        public override string Example => "switch (expr)";

        public override string Suffix => "switch";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"switch ({expression}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    case value:{Environment.NewLine}{indent}        break;{Environment.NewLine}{indent}}}";
        }
    }
}
