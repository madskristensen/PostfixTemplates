namespace PostfixTemplates.Templates
{
    internal sealed class FieldTemplate : PostfixTemplate
    {
        public override string Name => "field";

        public override string Description => "Introduces field for expression";

        public override string Example => "_field = expr;";

        public override string Suffix => "field";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"_field = {expression};";
        }
    }
}
