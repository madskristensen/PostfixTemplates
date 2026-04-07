using System;

namespace PostfixTemplates.Templates
{
    internal sealed class IfTemplate : PostfixTemplate
    {
        public override string Name => "if";

        public override string Description => "Checks boolean expression to be 'true'";

        public override string Example => "if (expr) { }";

        public override string Suffix => "if statement";

        public override ExpressionType ApplicableTypes => ExpressionType.Boolean;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"if ({expression}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
