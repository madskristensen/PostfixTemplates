namespace PostfixTemplates.Templates
{
    internal sealed class ParseTemplate : PostfixTemplate
    {
        public override string Name => "parse";

        public override string Description => "Parses string as value of some type";

        public override string Example => "int.Parse(expr)";

        public override string Suffix => "parse";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"int.Parse({expression})";
        }
    }
}
