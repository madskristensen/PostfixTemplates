namespace PostfixTemplates.Templates
{
    internal sealed class NewTemplate : PostfixTemplate
    {
        public override string Name => "new";

        public override string Description => "Produces instantiation expression for type";

        public override string Example => "new SomeType()";

        public override string Suffix => "new";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"new {expression}()";
        }
    }
}
