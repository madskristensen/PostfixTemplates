namespace PostfixTemplates.Templates
{
    internal sealed class DiscardTemplate : PostfixTemplate
    {
        public override string Name => "discard";

        public override string Description => "Discards expression result";

        public override string Example => "_ = expr;";

        public override string Suffix => "discard";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"_ = {expression};";
        }
    }
}
