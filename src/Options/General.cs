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

        [Category("Templates")]
        [DisplayName("Enable '.arg'")]
        [Description("Surrounds expression with invocation")]
        [DefaultValue(true)]
        public bool EnableArg { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.await'")]
        [Description("Awaits expressions of 'Task' type")]
        [DefaultValue(true)]
        public bool EnableAwait { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.cast'")]
        [Description("Surrounds expression with cast")]
        [DefaultValue(true)]
        public bool EnableCast { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.field'")]
        [Description("Introduces field for expression")]
        [DefaultValue(true)]
        public bool EnableField { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.for'")]
        [Description("Iterates over collection with index")]
        [DefaultValue(true)]
        public bool EnableFor { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.forr'")]
        [Description("Iterates over collection in reverse with index")]
        [DefaultValue(true)]
        public bool EnableForR { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.inject'")]
        [Description("Introduces primary constructor parameter of type")]
        [DefaultValue(true)]
        public bool EnableInject { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.lock'")]
        [Description("Surrounds expression with lock block")]
        [DefaultValue(true)]
        public bool EnableLock { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.new'")]
        [Description("Produces instantiation expression for type")]
        [DefaultValue(true)]
        public bool EnableNew { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.par'")]
        [Description("Parenthesizes current expression")]
        [DefaultValue(true)]
        public bool EnablePar { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.parse'")]
        [Description("Parses string as value of some type")]
        [DefaultValue(true)]
        public bool EnableParse { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.prop'")]
        [Description("Introduces property for expression")]
        [DefaultValue(true)]
        public bool EnableProp { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.switch'")]
        [Description("Produces switch statement")]
        [DefaultValue(true)]
        public bool EnableSwitch { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.to'")]
        [Description("Assigns current expression to some variable")]
        [DefaultValue(true)]
        public bool EnableTo { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.tryparse'")]
        [Description("Parses string as value of some type")]
        [DefaultValue(true)]
        public bool EnableTryParse { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.typeof'")]
        [Description("Wraps type usage with typeof() expression")]
        [DefaultValue(true)]
        public bool EnableTypeof { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.using'")]
        [Description("Wraps resource with using statement")]
        [DefaultValue(true)]
        public bool EnableUsing { get; set; } = true;

        [Category("Templates")]
        [DisplayName("Enable '.yield'")]
        [Description("Yields value from iterator method")]
        [DefaultValue(true)]
        public bool EnableYield { get; set; } = true;

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
                "arg" => EnableArg,
                "await" => EnableAwait,
                "cast" => EnableCast,
                "field" => EnableField,
                "for" => EnableFor,
                "forr" => EnableForR,
                "inject" => EnableInject,
                "lock" => EnableLock,
                "new" => EnableNew,
                "par" => EnablePar,
                "parse" => EnableParse,
                "prop" => EnableProp,
                "switch" => EnableSwitch,
                "to" => EnableTo,
                "tryparse" => EnableTryParse,
                "typeof" => EnableTypeof,
                "using" => EnableUsing,
                "yield" => EnableYield,
                _ => false
            };
        }
    }
}
