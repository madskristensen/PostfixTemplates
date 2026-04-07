namespace PostfixTemplates.Templates
{
    internal sealed class TypeofTemplate : PostfixTemplate
    {
        public override string Name => "typeof";

        public override string Description => "Wraps type usage with typeof() expression";

        public override string Example => "typeof(TExpr)";

        public override string Suffix => "typeof";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"typeof({expression})";
        }
    }
}
