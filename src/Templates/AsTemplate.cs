namespace PostfixTemplates.Templates
{
    internal sealed class AsTemplate : PostfixTemplate
    {
        public override string Name => "as";

        public override string Description => "Casts expression using safe 'as' operator";

        public override string Example => "expr as SomeType";

        public override string Suffix => "safe cast";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "SomeType";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"{expression} as SomeType";
        }
    }
}
