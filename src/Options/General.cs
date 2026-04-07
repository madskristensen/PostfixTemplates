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
        // Control Flow

        [Category("Control Flow")]
        [DisplayName("Enable '.if'")]
        [Description("Checks boolean expression to be 'true'")]
        [DefaultValue(true)]
        public bool EnableIf { get; set; } = true;

        [Category("Control Flow")]
        [DisplayName("Enable '.else'")]
        [Description("Checks boolean expression to be 'false'")]
        [DefaultValue(true)]
        public bool EnableElse { get; set; } = true;

        [Category("Control Flow")]
        [DisplayName("Enable '.switch'")]
        [Description("Produces switch statement")]
        [DefaultValue(true)]
        public bool EnableSwitch { get; set; } = true;

        [Category("Control Flow")]
        [DisplayName("Enable '.switchexpr'")]
        [Description("Produces switch expression")]
        [DefaultValue(true)]
        public bool EnableSwitchExpr { get; set; } = true;

        [Category("Control Flow")]
        [DisplayName("Enable '.conditional'")]
        [Description("Wraps expression with ternary conditional operator")]
        [DefaultValue(true)]
        public bool EnableConditional { get; set; } = true;

        // Loops

        [Category("Loops")]
        [DisplayName("Enable '.for'")]
        [Description("Iterates over collection with index")]
        [DefaultValue(true)]
        public bool EnableFor { get; set; } = true;

        [Category("Loops")]
        [DisplayName("Enable '.forr'")]
        [Description("Iterates over collection in reverse with index")]
        [DefaultValue(true)]
        public bool EnableForR { get; set; } = true;

        [Category("Loops")]
        [DisplayName("Enable '.foreach'")]
        [Description("Iterates over enumerable collection")]
        [DefaultValue(true)]
        public bool EnableForEach { get; set; } = true;

        [Category("Loops")]
        [DisplayName("Enable '.while'")]
        [Description("Iterates while boolean expression is 'true'")]
        [DefaultValue(true)]
        public bool EnableWhile { get; set; } = true;

        // Null and Empty Checks

        [Category("Null and Empty Checks")]
        [DisplayName("Enable '.null'")]
        [Description("Checks expression to be null")]
        [DefaultValue(true)]
        public bool EnableNull { get; set; } = true;

        [Category("Null and Empty Checks")]
        [DisplayName("Enable '.notnull'")]
        [Description("Checks expression to be not null")]
        [DefaultValue(true)]
        public bool EnableNotNull { get; set; } = true;

        [Category("Null and Empty Checks")]
        [DisplayName("Enable '.throwifnull'")]
        [Description("Throws if expression is null")]
        [DefaultValue(true)]
        public bool EnableThrowIfNull { get; set; } = true;

        [Category("Null and Empty Checks")]
        [DisplayName("Enable '.notempty'")]
        [Description("Checks if string is not null or empty")]
        [DefaultValue(true)]
        public bool EnableNotEmpty { get; set; } = true;

        [Category("Null and Empty Checks")]
        [DisplayName("Enable '.notwhitespace'")]
        [Description("Checks if string is not null or whitespace")]
        [DefaultValue(true)]
        public bool EnableNotWhitespace { get; set; } = true;

        // Type Operations

        [Category("Type Operations")]
        [DisplayName("Enable '.cast'")]
        [Description("Surrounds expression with cast")]
        [DefaultValue(true)]
        public bool EnableCast { get; set; } = true;

        [Category("Type Operations")]
        [DisplayName("Enable '.is'")]
        [Description("Checks expression type with pattern matching")]
        [DefaultValue(true)]
        public bool EnableIs { get; set; } = true;

        [Category("Type Operations")]
        [DisplayName("Enable '.as'")]
        [Description("Casts expression using safe 'as' operator")]
        [DefaultValue(true)]
        public bool EnableAs { get; set; } = true;

        [Category("Type Operations")]
        [DisplayName("Enable '.typeof'")]
        [Description("Wraps type usage with typeof() expression")]
        [DefaultValue(true)]
        public bool EnableTypeof { get; set; } = true;

        [Category("Type Operations")]
        [DisplayName("Enable '.new'")]
        [Description("Produces instantiation expression for type")]
        [DefaultValue(true)]
        public bool EnableNew { get; set; } = true;

        [Category("Type Operations")]
        [DisplayName("Enable '.parse'")]
        [Description("Parses string as value of some type")]
        [DefaultValue(true)]
        public bool EnableParse { get; set; } = true;

        [Category("Type Operations")]
        [DisplayName("Enable '.tryparse'")]
        [Description("Parses string as value of some type")]
        [DefaultValue(true)]
        public bool EnableTryParse { get; set; } = true;

        // Variables and Members

        [Category("Variables and Members")]
        [DisplayName("Enable '.var'")]
        [Description("Introduces variable for expression")]
        [DefaultValue(true)]
        public bool EnableVar { get; set; } = true;

        [Category("Variables and Members")]
        [DisplayName("Enable '.field'")]
        [Description("Introduces field for expression")]
        [DefaultValue(true)]
        public bool EnableField { get; set; } = true;

        [Category("Variables and Members")]
        [DisplayName("Enable '.prop'")]
        [Description("Introduces property for expression")]
        [DefaultValue(true)]
        public bool EnableProp { get; set; } = true;

        [Category("Variables and Members")]
        [DisplayName("Enable '.to'")]
        [Description("Assigns current expression to some variable")]
        [DefaultValue(true)]
        public bool EnableTo { get; set; } = true;

        [Category("Variables and Members")]
        [DisplayName("Enable '.inject'")]
        [Description("Introduces primary constructor parameter of type")]
        [DefaultValue(true)]
        public bool EnableInject { get; set; } = true;

        // Expressions

        [Category("Expressions")]
        [DisplayName("Enable '.not'")]
        [Description("Negates boolean expression")]
        [DefaultValue(true)]
        public bool EnableNot { get; set; } = true;

        [Category("Expressions")]
        [DisplayName("Enable '.par'")]
        [Description("Parenthesizes current expression")]
        [DefaultValue(true)]
        public bool EnablePar { get; set; } = true;

        [Category("Expressions")]
        [DisplayName("Enable '.arg'")]
        [Description("Surrounds expression with invocation")]
        [DefaultValue(true)]
        public bool EnableArg { get; set; } = true;

        [Category("Expressions")]
        [DisplayName("Enable '.await'")]
        [Description("Awaits expressions of 'Task' type")]
        [DefaultValue(true)]
        public bool EnableAwait { get; set; } = true;

        [Category("Expressions")]
        [DisplayName("Enable '.discard'")]
        [Description("Discards expression result")]
        [DefaultValue(true)]
        public bool EnableDiscard { get; set; } = true;

        [Category("Expressions")]
        [DisplayName("Enable '.nameof'")]
        [Description("Wraps expression with nameof()")]
        [DefaultValue(true)]
        public bool EnableNameof { get; set; } = true;

        [Category("Expressions")]
        [DisplayName("Enable '.lambda'")]
        [Description("Wraps expression in a lambda")]
        [DefaultValue(true)]
        public bool EnableLambda { get; set; } = true;

        // Statements

        [Category("Statements")]
        [DisplayName("Enable '.return'")]
        [Description("Returns expression from current function")]
        [DefaultValue(true)]
        public bool EnableReturn { get; set; } = true;

        [Category("Statements")]
        [DisplayName("Enable '.throw'")]
        [Description("Throws expression of 'Exception' type")]
        [DefaultValue(true)]
        public bool EnableThrow { get; set; } = true;

        [Category("Statements")]
        [DisplayName("Enable '.trycatch'")]
        [Description("Wraps expression with try/catch block")]
        [DefaultValue(true)]
        public bool EnableTryCatch { get; set; } = true;

        [Category("Statements")]
        [DisplayName("Enable '.using'")]
        [Description("Wraps resource with using statement")]
        [DefaultValue(true)]
        public bool EnableUsing { get; set; } = true;

        [Category("Statements")]
        [DisplayName("Enable '.lock'")]
        [Description("Surrounds expression with lock block")]
        [DefaultValue(true)]
        public bool EnableLock { get; set; } = true;

        [Category("Statements")]
        [DisplayName("Enable '.yield'")]
        [Description("Yields value from iterator method")]
        [DefaultValue(true)]
        public bool EnableYield { get; set; } = true;

        // Diagnostics

        [Category("Diagnostics")]
        [DisplayName("Enable '.writeline'")]
        [Description("Writes expression to console output")]
        [DefaultValue(true)]
        public bool EnableWriteLine { get; set; } = true;

        [Category("Diagnostics")]
        [DisplayName("Enable '.assert'")]
        [Description("Asserts boolean expression with Debug.Assert")]
        [DefaultValue(true)]
        public bool EnableAssert { get; set; } = true;

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
                "writeline" => EnableWriteLine,
                "assert" => EnableAssert,
                "is" => EnableIs,
                "as" => EnableAs,
                "switchexpr" => EnableSwitchExpr,
                "discard" => EnableDiscard,
                "nameof" => EnableNameof,
                "lambda" => EnableLambda,
                "conditional" => EnableConditional,
                "trycatch" => EnableTryCatch,
                "throwifnull" => EnableThrowIfNull,
                "notempty" => EnableNotEmpty,
                "notwhitespace" => EnableNotWhitespace,
                _ => false
            };
        }
    }
}
