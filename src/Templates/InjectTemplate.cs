namespace PostfixTemplates.Templates
{
    internal sealed class InjectTemplate : PostfixTemplate
    {
        public override string Name => "inject";

        public override string Description => "Introduces primary constructor parameter of type";

        public override string Example => "class Component(IDependency dependency) { }";

        public override string Suffix => "primary constructor";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override bool RequiresValueExpression => false;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"({expression} dependency)";
        }
    }
}
