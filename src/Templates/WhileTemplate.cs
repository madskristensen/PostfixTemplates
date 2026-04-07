using System;

namespace PostfixTemplates.Templates
{
    internal sealed class WhileTemplate : PostfixTemplate
    {
        public override string Name => "while";

        public override string Description => "Iterates while boolean expression is 'true'";

        public override string Example => "while (expr) { }";

        public override string Suffix => "while loop";

        public override ExpressionType ApplicableTypes => ExpressionType.Boolean;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"while ({expression}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
