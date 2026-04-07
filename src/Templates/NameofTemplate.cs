namespace PostfixTemplates.Templates
{
    internal sealed class NameofTemplate : PostfixTemplate
    {
        public override string Name => "nameof";

        public override string Description => "Wraps expression with nameof()";

        public override string Example => "nameof(expr)";

        public override string Suffix => "nameof";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override bool RequiresValueExpression => false;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"nameof({expression})";
        }
    }
}
