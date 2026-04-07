namespace PostfixTemplates.Templates
{
    internal sealed class TryParseTemplate : PostfixTemplate
    {
        public override string Name => "tryparse";

        public override string Description => "Parses string as value of some type";

        public override string Example => "int.TryParse(expr, out value)";

        public override string Suffix => "try parse";

        public override ExpressionType ApplicableTypes => ExpressionType.String;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"int.TryParse({expression}, out var value)";
        }
    }
}
