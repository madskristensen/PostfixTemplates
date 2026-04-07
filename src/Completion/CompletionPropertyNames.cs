namespace PostfixTemplates.Completion
{
    /// <summary>
    /// Shared constants for property-bag keys used to pass data between
    /// <see cref="PostfixCompletionSource"/> and <see cref="PostfixCompletionCommitManager"/>.
    /// </summary>
    internal static class CompletionPropertyNames
    {
        public const string PostfixTemplate = "PostfixTemplate";
        public const string ExpressionText = "ExpressionText";
        public const string ExpressionStart = "ExpressionStart";
        public const string DotPosition = "DotPosition";
    }
}
