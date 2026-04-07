namespace PostfixTemplates.Templates
{
    internal sealed class ThrowIfNullTemplate : PostfixTemplate
    {
        public override string Name => "throwifnull";

        public override string Description => "Throws if expression is null";

        public override string Example => "ArgumentNullException.ThrowIfNull(expr)";

        public override string Suffix => "null guard";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"ArgumentNullException.ThrowIfNull({expression})";
        }
    }
}
