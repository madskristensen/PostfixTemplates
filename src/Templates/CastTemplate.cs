namespace PostfixTemplates.Templates
{
    internal sealed class CastTemplate : PostfixTemplate
    {
        public override string Name => "cast";

        public override string Description => "Surrounds expression with cast";

        public override string Example => "((SomeType) expr)";

        public override string Suffix => "cast";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "SomeType";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"((SomeType) {expression})";
        }
    }
}
