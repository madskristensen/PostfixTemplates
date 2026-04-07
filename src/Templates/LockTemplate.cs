using System;

namespace PostfixTemplates.Templates
{
    internal sealed class LockTemplate : PostfixTemplate
    {
        public override string Name => "lock";

        public override string Description => "Surrounds expression with lock block";

        public override string Example => "lock (expr) { }";

        public override string Suffix => "lock";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"lock ({expression}){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
