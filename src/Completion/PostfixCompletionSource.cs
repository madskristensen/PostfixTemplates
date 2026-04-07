using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
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

                // Get existing completion items from Roslyn to avoid duplicates
                HashSet<string> existingItemNames = await GetExistingCompletionItemNamesAsync(document, triggerLocation.Position, cancellationToken);

                ImmutableArray<VsCompletionItem>.Builder items = ImmutableArray.CreateBuilder<VsCompletionItem>();

                foreach (PostfixTemplate template in PostfixTemplate.All)
                {
                    if (!settings.IsTemplateEnabled(template.Name))
                    {
                        continue;
                    }

                    // Skip if Roslyn already provides a completion item with the same name
                    if (existingItemNames.Contains(template.Name))
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
                    if (template.RequiresAsyncContext && !RoslynExpressionHelper.IsInAsyncContext(tree, dotPosition, cancellationToken))
                    {
                        continue;
                    }

                    // Skip templates that require an iterator context when not in an iterator method
                    if (template.RequiresIteratorContext && !RoslynExpressionHelper.IsInIteratorContext(tree, dotPosition, semanticModel, cancellationToken))
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
                && PostfixTemplate.ByName.TryGetValue(templateName, out PostfixTemplate template))
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
        /// Queries the Roslyn completion service to get the names of completion items
        /// that would be provided at the current position. This allows us to avoid
        /// showing duplicate items when VS already provides a built-in postfix template.
        /// </summary>
        private static async Task<HashSet<string>> GetExistingCompletionItemNamesAsync(Document document, int position, CancellationToken cancellationToken)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var completionService = CompletionService.GetService(document);

                if (completionService == null)
                {
                    return result;
                }

                CompletionList completions = await completionService.GetCompletionsAsync(document, position, cancellationToken: cancellationToken);

                if (completions == null)
                {
                    return result;
                }

                foreach (Microsoft.CodeAnalysis.Completion.CompletionItem item in completions.Items)
                {
                    result.Add(item.DisplayText);
                }
            }
            catch (Exception ex)
            {
                await ex.LogAsync();
            }

            return result;
        }
    }
}
