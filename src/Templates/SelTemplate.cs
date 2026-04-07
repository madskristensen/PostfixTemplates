namespace PostfixTemplates.Templates
{
    internal sealed class SelTemplate : PostfixTemplate
    {
        public override string Name => "sel";

        public override string Description => "Selects expression in editor";

        public override string Example => "|selected + expression|";

        public override string Suffix => "select";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return expression;
        }
    }
}
