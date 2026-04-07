using System;

namespace PostfixTemplates.Templates
{
    internal sealed class TryCatchTemplate : PostfixTemplate
    {
        public override string Name => "trycatch";

        public override string Description => "Wraps expression with try/catch block";

        public override string Example => "try { expr; } catch { }";

        public override string Suffix => "try/catch";

        public override ExpressionType ApplicableTypes => ExpressionType.Any;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"try{Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {expression};{Environment.NewLine}{indent}}}{Environment.NewLine}{indent}catch (Exception ex){Environment.NewLine}{indent}{{{Environment.NewLine}{indent}    {Environment.NewLine}{indent}}}";
        }
    }
}
