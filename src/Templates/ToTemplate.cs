namespace PostfixTemplates.Templates
{
    internal sealed class ToTemplate : PostfixTemplate
    {
        public override string Name => "to";

        public override string Description => "Assigns current expression to some variable";

        public override string Example => "lvalue = expr;";

        public override string Suffix => "assign";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "lvalue";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"lvalue = {expression};";
        }
    }
}
