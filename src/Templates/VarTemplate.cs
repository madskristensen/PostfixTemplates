namespace PostfixTemplates.Templates
{
    internal sealed class VarTemplate : PostfixTemplate
    {
        public override string Name => "var";

        public override string Description => "Introduces variable for expression";

        public override string Example => "var x = expr;";

        public override string Suffix => "variable";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "x";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"var x = {expression};";
        }
    }
}
