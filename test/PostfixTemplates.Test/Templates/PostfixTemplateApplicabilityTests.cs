using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Test.Templates;

[TestClass]
public class PostfixTemplateApplicabilityTests
{
    private static SemanticModel GetSemanticModel(string code)
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);
        return compilation.GetSemanticModel(tree);
    }

    private static ITypeSymbol GetTypeSymbol(string typeName)
    {
        var code = $@"
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
class Test {{
    void Method() {{
        {typeName} x = default;
    }}
}}";
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        CSharpCompilation compilation = CSharpCompilation.Create("Test")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddReferences(MetadataReference.CreateFromFile(typeof(System.Collections.Generic.List<>).Assembly.Location))
            .AddReferences(MetadataReference.CreateFromFile(typeof(System.IO.StreamReader).Assembly.Location))
            .AddReferences(MetadataReference.CreateFromFile(typeof(System.Threading.Tasks.Task).Assembly.Location))
            .AddSyntaxTrees(tree);
        SemanticModel semanticModel = compilation.GetSemanticModel(tree);

        SyntaxNode root = tree.GetRoot();
        VariableDeclarationSyntax variableDeclaration = root.DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax>()
            .First();

        return semanticModel.GetTypeInfo(variableDeclaration.Type).Type;
    }

    // ===================================================================
    // Type symbol helpers for the applicability matrix
    // ===================================================================
    private static ITypeSymbol BoolType => GetTypeSymbol("bool");
    private static ITypeSymbol IntType => GetTypeSymbol("int");
    private static ITypeSymbol StringType => GetTypeSymbol("string");
    private static ITypeSymbol ArrayType => GetTypeSymbol("int[]");
    private static ITypeSymbol ListType => GetTypeSymbol("List<int>");
    private static ITypeSymbol ExceptionType => GetTypeSymbol("Exception");
    private static ITypeSymbol ArgumentExceptionType => GetTypeSymbol("ArgumentException");
    private static ITypeSymbol TaskType => GetTypeSymbol("Task");
    private static ITypeSymbol StreamReaderType => GetTypeSymbol("StreamReader");
    private static ITypeSymbol NullableIntType => GetTypeSymbol("int?");

    // ===================================================================
    // IfTemplate - Boolean only
    // ===================================================================
    [TestMethod]
    public void IfTemplate_ApplicableToBoolean()
    {
        var template = new IfTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToString()
    {
        var template = new IfTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToInt()
    {
        var template = new IfTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToArray()
    {
        var template = new IfTemplate();
        Assert.IsFalse(template.IsApplicableToType(ArrayType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToException()
    {
        var template = new IfTemplate();
        Assert.IsFalse(template.IsApplicableToType(ExceptionType));
    }

    [TestMethod]
    public void IfTemplate_NotApplicableToTask()
    {
        var template = new IfTemplate();
        Assert.IsFalse(template.IsApplicableToType(TaskType));
    }

    // ===================================================================
    // ElseTemplate - Boolean only
    // ===================================================================
    [TestMethod]
    public void ElseTemplate_ApplicableToBoolean()
    {
        var template = new ElseTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void ElseTemplate_NotApplicableToString()
    {
        var template = new ElseTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void ElseTemplate_NotApplicableToInt()
    {
        var template = new ElseTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ElseTemplate_NotApplicableToArray()
    {
        var template = new ElseTemplate();
        Assert.IsFalse(template.IsApplicableToType(ArrayType));
    }

    // ===================================================================
    // NotTemplate - Boolean only
    // ===================================================================
    [TestMethod]
    public void NotTemplate_ApplicableToBoolean()
    {
        var template = new NotTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void NotTemplate_NotApplicableToString()
    {
        var template = new NotTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void NotTemplate_NotApplicableToInt()
    {
        var template = new NotTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void NotTemplate_NotApplicableToTask()
    {
        var template = new NotTemplate();
        Assert.IsFalse(template.IsApplicableToType(TaskType));
    }

    // ===================================================================
    // WhileTemplate - Boolean only
    // ===================================================================
    [TestMethod]
    public void WhileTemplate_ApplicableToBoolean()
    {
        var template = new WhileTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void WhileTemplate_NotApplicableToInt()
    {
        var template = new WhileTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void WhileTemplate_NotApplicableToString()
    {
        var template = new WhileTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void WhileTemplate_NotApplicableToArray()
    {
        var template = new WhileTemplate();
        Assert.IsFalse(template.IsApplicableToType(ArrayType));
    }

    // ===================================================================
    // NullTemplate - Nullable (reference types + Nullable<T>)
    // ===================================================================
    [TestMethod]
    public void NullTemplate_ApplicableToString()
    {
        var template = new NullTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void NullTemplate_ApplicableToNullableInt()
    {
        var template = new NullTemplate();
        Assert.IsTrue(template.IsApplicableToType(NullableIntType));
    }

    [TestMethod]
    public void NullTemplate_ApplicableToException()
    {
        var template = new NullTemplate();
        Assert.IsTrue(template.IsApplicableToType(ExceptionType));
    }

    [TestMethod]
    public void NullTemplate_NotApplicableToBoolean()
    {
        var template = new NullTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void NullTemplate_NotApplicableToInt()
    {
        var template = new NullTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    // ===================================================================
    // NotNullTemplate - Nullable (reference types + Nullable<T>)
    // ===================================================================
    [TestMethod]
    public void NotNullTemplate_ApplicableToString()
    {
        var template = new NotNullTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void NotNullTemplate_ApplicableToNullableInt()
    {
        var template = new NotNullTemplate();
        Assert.IsTrue(template.IsApplicableToType(NullableIntType));
    }

    [TestMethod]
    public void NotNullTemplate_NotApplicableToBoolean()
    {
        var template = new NotNullTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void NotNullTemplate_NotApplicableToInt()
    {
        var template = new NotNullTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    // ===================================================================
    // ForEachTemplate - Enumerable only
    // ===================================================================
    [TestMethod]
    public void ForEachTemplate_ApplicableToArray()
    {
        var template = new ForEachTemplate();
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
    }

    [TestMethod]
    public void ForEachTemplate_ApplicableToListOfT()
    {
        var template = new ForEachTemplate();
        Assert.IsTrue(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void ForEachTemplate_ApplicableToString()
    {
        // string implements IEnumerable<char>
        var template = new ForEachTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToBoolean()
    {
        var template = new ForEachTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToInt()
    {
        var template = new ForEachTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToException()
    {
        var template = new ForEachTemplate();
        Assert.IsFalse(template.IsApplicableToType(ExceptionType));
    }

    [TestMethod]
    public void ForEachTemplate_NotApplicableToTask()
    {
        var template = new ForEachTemplate();
        Assert.IsFalse(template.IsApplicableToType(TaskType));
    }

    // ===================================================================
    // ForTemplate - Enumerable only
    // ===================================================================
    [TestMethod]
    public void ForTemplate_ApplicableToArray()
    {
        var template = new ForTemplate();
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
    }

    [TestMethod]
    public void ForTemplate_ApplicableToList()
    {
        var template = new ForTemplate();
        Assert.IsTrue(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void ForTemplate_NotApplicableToInt()
    {
        var template = new ForTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ForTemplate_NotApplicableToBoolean()
    {
        var template = new ForTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void ForTemplate_NotApplicableToException()
    {
        var template = new ForTemplate();
        Assert.IsFalse(template.IsApplicableToType(ExceptionType));
    }

    // ===================================================================
    // ForRTemplate - Enumerable only
    // ===================================================================
    [TestMethod]
    public void ForRTemplate_ApplicableToArray()
    {
        var template = new ForRTemplate();
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
    }

    [TestMethod]
    public void ForRTemplate_ApplicableToList()
    {
        var template = new ForRTemplate();
        Assert.IsTrue(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void ForRTemplate_NotApplicableToInt()
    {
        var template = new ForRTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ForRTemplate_NotApplicableToBoolean()
    {
        var template = new ForRTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    // ===================================================================
    // ThrowTemplate - Exception only
    // ===================================================================
    [TestMethod]
    public void ThrowTemplate_ApplicableToException()
    {
        var template = new ThrowTemplate();
        Assert.IsTrue(template.IsApplicableToType(ExceptionType));
    }

    [TestMethod]
    public void ThrowTemplate_ApplicableToDerivedExceptionType()
    {
        var template = new ThrowTemplate();
        Assert.IsTrue(template.IsApplicableToType(ArgumentExceptionType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToString()
    {
        var template = new ThrowTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToInt()
    {
        var template = new ThrowTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToBoolean()
    {
        var template = new ThrowTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToArray()
    {
        var template = new ThrowTemplate();
        Assert.IsFalse(template.IsApplicableToType(ArrayType));
    }

    [TestMethod]
    public void ThrowTemplate_NotApplicableToTask()
    {
        var template = new ThrowTemplate();
        Assert.IsFalse(template.IsApplicableToType(TaskType));
    }

    // ===================================================================
    // AwaitTemplate - Awaitable only
    // ===================================================================
    [TestMethod]
    public void AwaitTemplate_ApplicableToTask()
    {
        var template = new AwaitTemplate();
        Assert.IsTrue(template.IsApplicableToType(TaskType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToString()
    {
        var template = new AwaitTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToList()
    {
        var template = new AwaitTemplate();
        Assert.IsFalse(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToInt()
    {
        var template = new AwaitTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToBoolean()
    {
        var template = new AwaitTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void AwaitTemplate_NotApplicableToException()
    {
        var template = new AwaitTemplate();
        Assert.IsFalse(template.IsApplicableToType(ExceptionType));
    }

    // ===================================================================
    // UsingTemplate - Disposable only
    // ===================================================================
    [TestMethod]
    public void UsingTemplate_ApplicableToDisposableType()
    {
        var template = new UsingTemplate();
        Assert.IsTrue(template.IsApplicableToType(StreamReaderType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToString()
    {
        var template = new UsingTemplate();
        Assert.IsFalse(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToList()
    {
        var template = new UsingTemplate();
        Assert.IsFalse(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToInt()
    {
        var template = new UsingTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToBoolean()
    {
        var template = new UsingTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void UsingTemplate_NotApplicableToException()
    {
        var template = new UsingTemplate();
        Assert.IsFalse(template.IsApplicableToType(ExceptionType));
    }

    // ===================================================================
    // LockTemplate - ReferenceType only
    // ===================================================================
    [TestMethod]
    public void LockTemplate_ApplicableToString()
    {
        var template = new LockTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void LockTemplate_ApplicableToList()
    {
        var template = new LockTemplate();
        Assert.IsTrue(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void LockTemplate_ApplicableToException()
    {
        var template = new LockTemplate();
        Assert.IsTrue(template.IsApplicableToType(ExceptionType));
    }

    [TestMethod]
    public void LockTemplate_NotApplicableToInt()
    {
        var template = new LockTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void LockTemplate_NotApplicableToBoolean()
    {
        var template = new LockTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void LockTemplate_NotApplicableToNullableInt()
    {
        var template = new LockTemplate();
        Assert.IsFalse(template.IsApplicableToType(NullableIntType));
    }

    // ===================================================================
    // ParseTemplate - String only
    // ===================================================================
    [TestMethod]
    public void ParseTemplate_ApplicableToString()
    {
        var template = new ParseTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void ParseTemplate_NotApplicableToInt()
    {
        var template = new ParseTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ParseTemplate_NotApplicableToList()
    {
        var template = new ParseTemplate();
        Assert.IsFalse(template.IsApplicableToType(ListType));
    }

    [TestMethod]
    public void ParseTemplate_NotApplicableToBoolean()
    {
        var template = new ParseTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    // ===================================================================
    // TryParseTemplate - String only
    // ===================================================================
    [TestMethod]
    public void TryParseTemplate_ApplicableToString()
    {
        var template = new TryParseTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void TryParseTemplate_NotApplicableToInt()
    {
        var template = new TryParseTemplate();
        Assert.IsFalse(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void TryParseTemplate_NotApplicableToBoolean()
    {
        var template = new TryParseTemplate();
        Assert.IsFalse(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void TryParseTemplate_NotApplicableToArray()
    {
        var template = new TryParseTemplate();
        Assert.IsFalse(template.IsApplicableToType(ArrayType));
    }

    // ===================================================================
    // Any-type templates: arg, cast, field, par, prop, return, switch, to, var
    // These should apply to every type
    // ===================================================================
    [TestMethod]
    public void ReturnTemplate_ApplicableToAny()
    {
        var template = new ReturnTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
        Assert.IsTrue(template.IsApplicableToType(ExceptionType));
        Assert.IsTrue(template.IsApplicableToType(TaskType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToBoolean()
    {
        var template = new VarTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToString()
    {
        var template = new VarTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void VarTemplate_ApplicableToInt()
    {
        var template = new VarTemplate();
        Assert.IsTrue(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ArgTemplate_ApplicableToAny()
    {
        var template = new ArgTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
        Assert.IsTrue(template.IsApplicableToType(ExceptionType));
    }

    [TestMethod]
    public void CastTemplate_ApplicableToAny()
    {
        var template = new CastTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
    }

    [TestMethod]
    public void FieldTemplate_ApplicableToAny()
    {
        var template = new FieldTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void ParTemplate_ApplicableToAny()
    {
        var template = new ParTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void PropTemplate_ApplicableToAny()
    {
        var template = new PropTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void SwitchTemplate_ApplicableToBoolean()
    {
        var template = new SwitchTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
    }

    [TestMethod]
    public void SwitchTemplate_ApplicableToString()
    {
        var template = new SwitchTemplate();
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    [TestMethod]
    public void SwitchTemplate_ApplicableToInt()
    {
        var template = new SwitchTemplate();
        Assert.IsTrue(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void ToTemplate_ApplicableToAny()
    {
        var template = new ToTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
    }

    // Type-only templates: new, typeof, inject
    [TestMethod]
    public void NewTemplate_ApplicableToAny()
    {
        var template = new NewTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void TypeofTemplate_ApplicableToAny()
    {
        var template = new TypeofTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
    }

    [TestMethod]
    public void InjectTemplate_ApplicableToAny()
    {
        var template = new InjectTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
    }

    // ===================================================================
    // Null-type fallback: all templates show when type is unknown
    // ===================================================================
    [TestMethod]
    public void AllTemplates_ApplicableWhenTypeIsNull()
    {
        // When we can't determine the type, templates should still show
        foreach (PostfixTemplate template in PostfixTemplate.All)
        {
            Assert.IsTrue(template.IsApplicableToType(null), $"Template '{template.Name}' should be applicable when type is null");
        }
    }

    [TestMethod]
    public void AllTemplates_HaveSuffix()
    {
        foreach (PostfixTemplate template in PostfixTemplate.All)
        {
            Assert.IsFalse(string.IsNullOrEmpty(template.Suffix), $"Template '{template.Name}' should have a suffix");
        }
    }

    // ===================================================================
    // RequiresValueExpression flag
    // ===================================================================
    [TestMethod]
    public void RequiresValueExpression_TrueByDefault()
    {
        // Most templates should require a value expression
        Assert.IsTrue(new NullTemplate().RequiresValueExpression);
        Assert.IsTrue(new NotNullTemplate().RequiresValueExpression);
        Assert.IsTrue(new ReturnTemplate().RequiresValueExpression);
        Assert.IsTrue(new VarTemplate().RequiresValueExpression);
    }

    [TestMethod]
    public void RequiresValueExpression_FalseForTypeTemplates()
    {
        // Templates designed for type names must not require a value expression
        Assert.IsFalse(new InjectTemplate().RequiresValueExpression);
        Assert.IsFalse(new NewTemplate().RequiresValueExpression);
        Assert.IsFalse(new TypeofTemplate().RequiresValueExpression);
    }

    // ===================================================================
    // YieldTemplate - Any type, but requires iterator context
    // ===================================================================
    [TestMethod]
    public void YieldTemplate_ApplicableToAny()
    {
        var template = new YieldTemplate();
        Assert.IsTrue(template.IsApplicableToType(BoolType));
        Assert.IsTrue(template.IsApplicableToType(IntType));
        Assert.IsTrue(template.IsApplicableToType(StringType));
        Assert.IsTrue(template.IsApplicableToType(ArrayType));
    }
}
