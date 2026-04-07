namespace PostfixTemplates.Templates
{
    internal sealed class PropTemplate : PostfixTemplate
    {
        public override string Name => "prop";

        public override string Description => "Introduces property for expression";

        public override string Example => "Property = expr;";

        public override string Suffix => "property";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "Property";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"Property = {expression};";
        }
    }
}
