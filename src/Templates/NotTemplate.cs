namespace PostfixTemplates.Templates
{
    internal sealed class NotTemplate : PostfixTemplate
    {
        public override string Name => "not";

        public override string Description => "Negates boolean expression";

        public override string Example => "!expr";

        public override string Suffix => "negate";

        public override ExpressionType ApplicableTypes => ExpressionType.Boolean;

        public override string GetTransformedText(string expression, string indent)
        {
            if (NeedsParenthesesForNegation(expression))
            {
                return $"!({expression})";
            }

            return $"!{expression}";
        }
    }
}
