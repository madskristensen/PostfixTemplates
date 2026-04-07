namespace PostfixTemplates.Templates
{
    internal sealed class ParTemplate : PostfixTemplate
    {
        public override string Name => "par";

        public override string Description => "Parenthesizes current expression";

        public override string Example => "(expr)";

        public override string Suffix => "parentheses";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"({expression})";
        }
    }
}
