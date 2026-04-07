using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PostfixTemplates
{
    internal partial class OptionsProvider
    {
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General>
        {
        }
    }

    public class General : BaseOptionModel<General>
    {
        [Category("Templates")]
        [DisplayName("Enable '.if'")]
        [Description("Checks boolean expression to be 'true'")]
        [DefaultValue(true)]
        public bool EnableIf { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.else'")]
        [Description("Checks boolean expression to be 'false'")]
        [DefaultValue(true)]
        public bool EnableElse { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.var'")]
        [Description("Introduces variable for expression")]
        [DefaultValue(true)]
        public bool EnableVar { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.not'")]
        [Description("Negates boolean expression")]
        [DefaultValue(true)]
        public bool EnableNot { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.null'")]
        [Description("Checks expression to be null")]
        [DefaultValue(true)]
        public bool EnableNull { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.notnull'")]
        [Description("Checks expression to be not null")]
        [DefaultValue(true)]
        public bool EnableNotNull { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.return'")]
        [Description("Returns expression from current function")]
        [DefaultValue(true)]
        public bool EnableReturn { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.foreach'")]
        [Description("Iterates over enumerable collection")]
        [DefaultValue(true)]
        public bool EnableForEach { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.while'")]
        [Description("Iterates while boolean expression is 'true'")]
        [DefaultValue(true)]
        public bool EnableWhile { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.throw'")]
        [Description("Throws expression of 'Exception' type")]
        [DefaultValue(true)]
        public bool EnableThrow { get; set; } = true;

        public bool IsTemplateEnabled(string templateName)
        {
            return templateName switch
            {
                "if" => EnableIf,
                "else" => EnableElse,
                "var" => EnableVar,
                "not" => EnableNot,
                "null" => EnableNull,
                "notnull" => EnableNotNull,
                "return" => EnableReturn,
                "foreach" => EnableForEach,
                "while" => EnableWhile,
                "throw" => EnableThrow,
                _ => false
            };
        }
    }
}
