namespace PostfixTemplates.Templates
{
    internal sealed class IsTemplate : PostfixTemplate
    {
        public override string Name => "is";

        public override string Description => "Checks expression type with pattern matching";

        public override string Example => "expr is SomeType name";

        public override string Suffix => "type check";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "SomeType";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"{expression} is SomeType name";
        }
    }
}
