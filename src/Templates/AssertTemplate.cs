namespace PostfixTemplates.Templates
{
    internal sealed class AssertTemplate : PostfixTemplate
    {
        public override string Name => "assert";

        public override string Description => "Asserts boolean expression with Debug.Assert";

        public override string Example => "Debug.Assert(expr)";

        public override string Suffix => "debug assert";

        public override ExpressionType ApplicableTypes => ExpressionType.Boolean;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"Debug.Assert({expression})";
        }
    }
}
