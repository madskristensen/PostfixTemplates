namespace PostfixTemplates.Templates
{
    internal sealed class LambdaTemplate : PostfixTemplate
    {
        public override string Name => "lambda";

        public override string Description => "Wraps expression in a lambda";

        public override string Example => "x => expr";

        public override string Suffix => "lambda";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string SelectionPlaceholder => "x";

        public override string GetTransformedText(string expression, string indent)
        {
            return $"x => {expression}";
        }
    }
}
