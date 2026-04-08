using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using PostfixTemplates.Templates;

using VsCompletionContext = Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data.CompletionContext;
using VsCompletionItem = Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data.CompletionItem;
using VsCompletionTrigger = Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data.CompletionTrigger;

namespace PostfixTemplates.Completion
{
    internal sealed class PostfixCompletionSource : IAsyncCompletionSource
    {
        private static readonly ImageElement _icon = new(KnownMonikers.Snippet.ToImageId(), "Postfix Template");

        public CompletionStartData InitializeCompletion(VsCompletionTrigger trigger, SnapshotPoint triggerLocation, CancellationToken cancellationToken)
        {
            if (triggerLocation.Position == 0)
            {
                return CompletionStartData.DoesNotParticipateInCompletion;
            }

            ITextSnapshotLine line = triggerLocation.GetContainingLine();
            var lineText = line.GetText();
            var positionInLine = triggerLocation.Position - line.Start.Position;

            if (positionInLine > 0 && lineText[positionInLine - 1] == '.')
            {
                var applicableSpan = new SnapshotSpan(triggerLocation, 0);
                return new CompletionStartData(CompletionParticipation.ProvidesItems, applicableSpan);
            }

            if (positionInLine > 1 && lineText[positionInLine - 2] == '.')
            {
                var start = triggerLocation.Position - 1;
                var applicableSpan = new SnapshotSpan(triggerLocation.Snapshot, start, 1);
                return new CompletionStartData(CompletionParticipation.ProvidesItems, applicableSpan);
            }

            return CompletionStartData.DoesNotParticipateInCompletion;
        }

        public async Task<VsCompletionContext> GetCompletionContextAsync(IAsyncCompletionSession session, VsCompletionTrigger trigger, SnapshotPoint triggerLocation, SnapshotSpan applicableToSpan, CancellationToken cancellationToken)
        {
            try
            {
                ITextSnapshot snapshot = triggerLocation.Snapshot;
                Document document = snapshot.GetOpenDocumentInCurrentContextWithChanges();

                if (document == null)
                {
                    return VsCompletionContext.Empty;
                }

                SyntaxTree tree = await document.GetSyntaxTreeAsync(cancellationToken);

                if (tree == null)
                {
                    return VsCompletionContext.Empty;
                }

                var dotPosition = FindDotPosition(triggerLocation);

                if (dotPosition < 0)
                {
                    return VsCompletionContext.Empty;
                }

                RoslynExpressionHelper.ExpressionResult expressionResult = RoslynExpressionHelper.FindExpressionBeforeDot(tree, dotPosition, cancellationToken);

                if (expressionResult == null)
                {
                    return VsCompletionContext.Empty;
                }

                // Get the semantic model to determine the expression's type
                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken);
                ITypeSymbol expressionType = null;
                var isTypeExpression = false;

                if (semanticModel != null && expressionResult.ExpressionNode != null)
                {
                    TypeInfo typeInfo = semanticModel.GetTypeInfo(expressionResult.ExpressionNode, cancellationToken);
                    expressionType = typeInfo.Type;

                    // Detect whether the expression is a type reference (e.g. ExecutionContext.)
                    // rather than a value. GetTypeInfo returns null for a type expression;
                    // GetSymbolInfo resolves it to an ITypeSymbol instead.
                    if (expressionType == null)
                    {
                        SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(expressionResult.ExpressionNode, cancellationToken);
                        isTypeExpression = symbolInfo.Symbol is ITypeSymbol;
                    }
                }

                General settings = await General.GetLiveInstanceAsync();

                // Collect member names from the type to avoid duplicates with real members
                HashSet<string> existingMemberNames = GetMemberNames(expressionType);

                ImmutableArray<VsCompletionItem>.Builder items = ImmutableArray.CreateBuilder<VsCompletionItem>();

                foreach (PostfixTemplate template in PostfixTemplate.All)
                {
                    if (!settings.IsTemplateEnabled(template.Name))
                    {
                        continue;
                    }

                    // Skip if the type already has a member with the same name
                    if (existingMemberNames.Contains(template.Name))
                    {
                        continue;
                    }

                    // Skip if the template doesn't apply to this expression type
                    if (!template.IsApplicableToType(expressionType))
                    {
                        continue;
                    }

                    // Skip value-expression-only templates when the expression is a type reference
                    if (isTypeExpression && template.RequiresValueExpression)
                    {
                        continue;
                    }

                    // Skip type-only templates when the expression is a value (not a type reference)
                    if (!isTypeExpression && !template.RequiresValueExpression)
                    {
                        continue;
                    }

                    // Skip templates that require an async context when not in an async method/lambda
                    if (template.RequiresAsyncContext && !RoslynExpressionHelper.IsInAsyncContext(expressionResult.ExpressionNode))
                    {
                        continue;
                    }

                    // Skip templates that require an iterator context when not in an iterator method
                    if (template.RequiresIteratorContext && !RoslynExpressionHelper.IsInIteratorContext(expressionResult.ExpressionNode, semanticModel))
                    {
                        continue;
                    }

                    var item = new VsCompletionItem(template.Name, this, _icon, ImmutableArray<CompletionFilter>.Empty, template.Suffix);
                    item.Properties.AddProperty(CompletionPropertyNames.PostfixTemplate, template.Name);
                    item.Properties.AddProperty(CompletionPropertyNames.ExpressionText, expressionResult.Text);
                    item.Properties.AddProperty(CompletionPropertyNames.ExpressionStart, expressionResult.SpanStart);
                    item.Properties.AddProperty(CompletionPropertyNames.DotPosition, dotPosition);

                    items.Add(item);
                }

                // Add custom templates from .postfix.json
                foreach (PostfixTemplate custom in CustomTemplateLoader.Templates)
                {
                    if (existingMemberNames.Contains(custom.Name))
                    {
                        continue;
                    }

                    if (!custom.IsApplicableToType(expressionType))
                    {
                        continue;
                    }

                    if (isTypeExpression && custom.RequiresValueExpression)
                    {
                        continue;
                    }

                    var customItem = new VsCompletionItem(custom.Name, this, _icon, ImmutableArray<CompletionFilter>.Empty, custom.Suffix);
                    customItem.Properties.AddProperty(CompletionPropertyNames.PostfixTemplate, custom.Name);
                    customItem.Properties.AddProperty(CompletionPropertyNames.ExpressionText, expressionResult.Text);
                    customItem.Properties.AddProperty(CompletionPropertyNames.ExpressionStart, expressionResult.SpanStart);
                    customItem.Properties.AddProperty(CompletionPropertyNames.DotPosition, dotPosition);

                    items.Add(customItem);
                }

                return new VsCompletionContext(items.ToImmutable());
            }
            catch (Exception ex)
            {
                await ex.LogAsync();
                return VsCompletionContext.Empty;
            }
        }

        public Task<object> GetDescriptionAsync(IAsyncCompletionSession session, VsCompletionItem item, CancellationToken cancellationToken)
        {
            if (item.Properties.TryGetProperty(CompletionPropertyNames.PostfixTemplate, out string templateName)
                && PostfixTemplate.TryGetByName(templateName, out PostfixTemplate template))
            {
                var description = $"{template.Description}\n\nExample: {template.Example}";
                return Task.FromResult<object>(description);
            }

            return Task.FromResult<object>(null);
        }

        private int FindDotPosition(SnapshotPoint triggerLocation)
        {
            ITextSnapshotLine line = triggerLocation.GetContainingLine();
            var lineText = line.GetText();
            var positionInLine = triggerLocation.Position - line.Start.Position;

            for (var i = positionInLine - 1; i >= 0; i--)
            {
                if (lineText[i] == '.')
                {
                    return line.Start.Position + i;
                }

                if (!char.IsLetterOrDigit(lineText[i]) && lineText[i] != '_')
                {
                    break;
                }
            }

            return -1;
        }

        /// <summary>
        /// Collects member names from the expression's type and its base types so
        /// we can skip postfix templates whose name collides with an actual member.
        /// Much cheaper than invoking Roslyn's full completion pipeline.
        /// </summary>
        private static HashSet<string> GetMemberNames(ITypeSymbol typeSymbol)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (typeSymbol == null)
            {
                return result;
            }

            ITypeSymbol current = typeSymbol;

            while (current != null)
            {
                foreach (ISymbol member in current.GetMembers())
                {
                    if (!member.IsImplicitlyDeclared)
                    {
                        result.Add(member.Name);
                    }
                }

                current = current.BaseType;
            }

            return result;
        }
    }
}
