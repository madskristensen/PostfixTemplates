namespace PostfixTemplates.Templates
{
    internal sealed class YieldTemplate : PostfixTemplate
    {
        public override string Name => "yield";

        public override string Description => "Yields value from iterator method";

        public override string Example => "yield return expr;";

        public override string Suffix => "yield";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"yield return {expression};";
        }
    }
}
