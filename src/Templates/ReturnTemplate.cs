namespace PostfixTemplates.Templates
{
    internal sealed class ReturnTemplate : PostfixTemplate
    {
        public override string Name => "return";

        public override string Description => "Returns expression from current function";

        public override string Example => "return expr;";

        public override string Suffix => "return statement";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"return {expression};";
        }
    }
}
