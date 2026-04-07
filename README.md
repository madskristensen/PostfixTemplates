[marketplace]: <https://marketplace.visualstudio.com/items?itemName=MadsKristensen.PostfixTemplates>
[vsixgallery]: <https://www.vsixgallery.com/extension/PostfixTemplates.795137a8-0309-4bd6-a308-e85db689b742>
[repo]: <https://github.com/madskristensen/PostfixTemplates>

# Postfix Templates for Visual Studio

[![Build](https://github.com/madskristensen/PostfixTemplates/actions/workflows/build.yaml/badge.svg)](https://github.com/madskristensen/PostfixTemplates/actions/workflows/build.yaml)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/madskristensen)](https://github.com/sponsors/madskristensen)

Download this extension from the [Visual Studio Marketplace][marketplace]
or get the latest CI build from [Open VSIX Gallery][vsixgallery].

--------------------------------------

Postfix code completion templates for C#. Type an expression followed by a dot and a template shortcut to quickly transform it into a statement or wrap it in a common pattern.

## How it works

After typing a C# expression, press `.` and select a postfix template from the IntelliSense completion list. The expression is automatically transformed into the expanded form.

For example, typing `isValid.if` expands to:

```csharp
if (isValid)
{
}
```

Templates are context-aware — they only appear when applicable to the expression type.

## Templates

| Template | Applies to | Description | Example |
|----------|-----------|-------------|---------|
| `.if` | Boolean | Checks boolean expression to be 'true' | `expr.if` → `if (expr) { }` |
| `.else` | Boolean | Checks boolean expression to be 'false' | `expr.else` → `if (!expr) { }` |
| `.not` | Boolean | Negates boolean expression | `expr.not` → `!expr` |
| `.while` | Boolean | Iterates while boolean expression is 'true' | `expr.while` → `while (expr) { }` |
| `.assert` | Boolean | Asserts boolean expression with Debug.Assert | `expr.assert` → `Debug.Assert(expr)` |
| `.conditional` | Boolean | Wraps expression with ternary conditional operator | `expr.conditional` → `expr ? trueValue : falseValue` |
| `.var` | Any | Introduces variable for expression | `expr.var` → `var x = expr;` |
| `.return` | Any | Returns expression from current function | `expr.return` → `return expr;` |
| `.switch` | Any | Produces switch statement | `expr.switch` → `switch (expr) { }` |
| `.switchexpr` | Any | Produces switch expression | `expr.switchexpr` → `expr switch { }` |
| `.par` | Any | Parenthesizes current expression | `expr.par` → `(expr)` |
| `.cast` | Any | Surrounds expression with cast | `expr.cast` → `((SomeType) expr)` |
| `.is` | Any | Checks expression type with pattern matching | `expr.is` → `expr is SomeType name` |
| `.as` | Any | Casts expression using safe 'as' operator | `expr.as` → `expr as SomeType` |
| `.arg` | Any | Surrounds expression with invocation | `expr.arg` → `Method(expr)` |
| `.to` | Any | Assigns current expression to a variable | `expr.to` → `lvalue = expr;` |
| `.field` | Any | Introduces field for expression | `expr.field` → `_field = expr;` |
| `.prop` | Any | Introduces property for expression | `expr.prop` → `Property = expr;` |
| `.discard` | Any | Discards expression result | `expr.discard` → `_ = expr;` |
| `.lambda` | Any | Wraps expression in a lambda | `expr.lambda` → `x => expr` |
| `.writeline` | Any | Writes expression to console output | `expr.writeline` → `Console.WriteLine(expr)` |
| `.trycatch` | Any | Wraps expression with try/catch block | `expr.trycatch` → `try { expr; } catch { }` |
| `.throwifnull` | Any | Throws if expression is null | `expr.throwifnull` → `ArgumentNullException.ThrowIfNull(expr)` |
| `.null` | Nullable | Checks expression to be null | `expr.null` → `if (expr == null) { }` |
| `.notnull` | Nullable | Checks expression to be not null | `expr.notnull` → `if (expr != null) { }` |
| `.foreach` | Enumerable | Iterates over enumerable collection | `expr.foreach` → `foreach (var item in expr) { }` |
| `.for` | Enumerable | Iterates over collection with index | `expr.for` → `for (var i = 0; i < expr.Length; i++)` |
| `.forr` | Enumerable | Iterates over collection in reverse | `expr.forr` → `for (var i = expr.Length-1; i >= 0; i--)` |
| `.throw` | Exception | Throws expression of 'Exception' type | `expr.throw` → `throw expr;` |
| `.using` | Disposable | Wraps resource with using statement | `expr.using` → `using (expr) { }` |
| `.lock` | Reference type | Surrounds expression with lock block | `expr.lock` → `lock (expr) { }` |
| `.await` | Awaitable | Awaits expressions of 'Task' type | `expr.await` → `await expr` |
| `.yield` | Any (iterator) | Yields value from iterator method | `expr.yield` → `yield return expr;` |
| `.parse` | String | Parses string as value of some type | `expr.parse` → `int.Parse(expr)` |
| `.tryparse` | String | Tries parsing string as value of some type | `expr.tryparse` → `int.TryParse(expr, out var value)` |
| `.notempty` | String | Checks if string is null or empty | `expr.notempty` → `if (string.IsNullOrEmpty(expr)) { }` |
| `.notwhitespace` | String | Checks if string is null or whitespace | `expr.notwhitespace` → `if (string.IsNullOrWhiteSpace(expr)) { }` |
| `.new` | Type name | Produces instantiation expression for type | `SomeType.new` → `new SomeType()` |
| `.typeof` | Type name | Wraps type with typeof() expression | `SomeType.typeof` → `typeof(SomeType)` |
| `.nameof` | Type name | Wraps expression with nameof() | `SomeType.nameof` → `nameof(SomeType)` |
| `.inject` | Type name | Introduces primary constructor parameter | `IDependency.inject` → `(IDependency dependency)` |

## Configuration

Each template can be individually enabled or disabled from **Tools → Options → Postfix Templates → General**.

## Contribute

[Issues](https://github.com/madskristensen/PostfixTemplates/issues), ideas, and pull requests are welcome.
