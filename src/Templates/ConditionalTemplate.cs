namespace PostfixTemplates.Templates
{
    internal sealed class ConditionalTemplate : PostfixTemplate
    {
        public override string Name => "conditional";

        public override string Description => "Wraps expression with ternary conditional operator";

        public override string Example => "expr ? trueValue : falseValue";

        public override string Suffix => "ternary";

        public override ExpressionType ApplicableTypes => ExpressionType.Boolean;

        public override string SelectionPlaceholder => "trueValue";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"{expression} ? trueValue : falseValue";
        }
    }
}
