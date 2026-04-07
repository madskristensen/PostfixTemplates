namespace PostfixTemplates.Templates
{
    internal sealed class ArgTemplate : PostfixTemplate
    {
        public override string Name => "arg";

        public override string Description => "Surrounds expression with invocation";

        public override string Example => "Method(expr)";

        public override string Suffix => "as argument";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"Method({expression})";
        }
    }
}
