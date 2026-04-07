namespace PostfixTemplates.Templates
{
    internal sealed class AwaitTemplate : PostfixTemplate
    {
        public override string Name => "await";

        public override string Description => "Awaits expressions of 'Task' type";

        public override string Example => "await expr";

        public override string Suffix => "await";

        public override ExpressionType ApplicableTypes => ExpressionType.Awaitable;

        public override bool RequiresAsyncContext => true;

        public override string GetTransformedText(string expression, string indent)
        {
            return $"await {expression}";
        }
    }
}
