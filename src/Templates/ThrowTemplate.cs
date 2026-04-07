namespace PostfixTemplates.Templates
{
    internal sealed class ThrowTemplate : PostfixTemplate
    {
        public override string Name => "throw";

        public override string Description => "Throws expression of 'Exception' type";

        public override string Example => "throw expr;";

        public override string Suffix => "throw statement";

        public override ExpressionType ApplicableTypes => ExpressionType.Exception;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"throw {expression};";
        }
    }
}
