using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PostfixTemplates.Templates
{
    /// <summary>
    /// A user-defined postfix template loaded from a <c>.postfix.json</c> file
    /// at the solution root.
    /// </summary>
    internal sealed class CustomTemplate : PostfixTemplate
    {
        public CustomTemplate(CustomTemplateDefinition definition)
        {
            Name = definition.Name;
            Description = definition.Description ?? definition.Name;
            Suffix = definition.Suffix ?? "custom";
            Body = definition.Body;
            ApplicableTypes = ParseExpressionType(definition.AppliesTo);
        }

        public override string Name { get; }

        public override string Description { get; }

        public override string Example => Body.Replace("{expr}", "expr");

        public override string Suffix { get; }

        public override ExpressionType ApplicableTypes { get; }

        /// <summary>
        /// The raw body template containing <c>{expr}</c> placeholders.
        /// </summary>
        public string Body { get; }

        public override string GetTransformedText(string expression, string indent)
        {
            return Body.Replace("{expr}", expression);
        }

        private static ExpressionType ParseExpressionType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ExpressionType.Any;
            }

            if (Enum.TryParse(value, ignoreCase: true, out ExpressionType result))
            {
                return result;
            }

            return ExpressionType.Any;
        }
    }

    /// <summary>
    /// JSON-serializable definition for a single custom template entry
    /// in <c>.postfix.json</c>.
    /// </summary>
    internal sealed class CustomTemplateDefinition
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// Short text shown to the right of the completion item in the IntelliSense list.
        /// Defaults to <c>custom</c> when omitted.
        /// </summary>
        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        /// <summary>
        /// The expression type this template applies to.
        /// Valid values match the <see cref="ExpressionType"/> enum names:
        /// <c>any</c>, <c>boolean</c>, <c>string</c>, <c>nullable</c>,
        /// <c>enumerable</c>, <c>exception</c>, <c>disposable</c>,
        /// <c>awaitable</c>, <c>referenceType</c>.
        /// Defaults to <c>any</c> when omitted.
        /// </summary>
        [JsonProperty("appliesTo")]
        public string AppliesTo { get; set; }
    }

    /// <summary>
    /// Root object for the <c>.postfix.json</c> file.
    /// </summary>
    internal sealed class CustomTemplateFile
    {
        [JsonProperty("templates")]
        public CustomTemplateDefinition[] Templates { get; set; } = Array.Empty<CustomTemplateDefinition>();
    }
}
