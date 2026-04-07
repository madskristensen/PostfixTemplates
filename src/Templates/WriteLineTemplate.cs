namespace PostfixTemplates.Templates
{
    internal sealed class WriteLineTemplate : PostfixTemplate
    {
        public override string Name => "writeline";

        public override string Description => "Writes expression to console output";

        public override string Example => "Console.WriteLine(expr)";

        public override string Suffix => "console output";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"Console.WriteLine({expression})";
        }
    }
}
