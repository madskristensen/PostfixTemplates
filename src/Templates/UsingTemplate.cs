using System;

namespace PostfixTemplates.Templates
{
    internal sealed class UsingTemplate : PostfixTemplate
    {
        public override string Name => "using";

        public override string Description => "Wraps resource with using statement";

        public override string Example => "using (expr) { }";

        public override string Suffix => "using";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"using ({expression}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
