using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using PostfixTemplates.Templates;

namespace PostfixTemplates.Completion
{
    internal sealed class PostfixCompletionCommitManager : IAsyncCompletionCommitManager
    {
        private static readonly char[] _commitCharacters = { '\t', '\n' };
        private readonly ITextView _textView;

        public PostfixCompletionCommitManager(ITextView textView)
        {
            _textView = textView;
        }

        public IEnumerable<char> PotentialCommitCharacters => _commitCharacters;

        public bool ShouldCommitCompletion(IAsyncCompletionSession session, SnapshotPoint location, char typedChar, CancellationToken cancellationToken)
        {
            CompletionItem selectedItem = session.GetComputedItems(cancellationToken).SelectedItem;

            if (selectedItem == null)
            {
                return false;
            }

            if (!selectedItem.Properties.ContainsProperty("PostfixTemplate"))
            {
                return false;
            }

            return typedChar == '\t' || typedChar == '\n';
        }

        public CommitResult TryCommit(IAsyncCompletionSession session, ITextBuffer buffer, CompletionItem item, char typedChar, CancellationToken cancellationToken)
        {
            try
            {
                if (!item.Properties.TryGetProperty("PostfixTemplate", out string templateName))
                {
                    return CommitResult.Unhandled;
                }

                if (!item.Properties.TryGetProperty("ExpressionText", out string expressionText))
                {
                    return CommitResult.Unhandled;
                }

                if (!item.Properties.TryGetProperty("ExpressionStart", out int expressionStart))
                {
                    return CommitResult.Unhandled;
                }

                if (!item.Properties.TryGetProperty("DotPosition", out int dotPosition))
                {
                    return CommitResult.Unhandled;
                }

                if (!PostfixTemplate.ByName.TryGetValue(templateName, out PostfixTemplate template))
                {
                    return CommitResult.Unhandled;
                }

                ITextSnapshot snapshot = buffer.CurrentSnapshot;
                SnapshotSpan applicableSpan = session.ApplicableToSpan.GetSpan(snapshot);
                var endPosition = applicableSpan.End.Position;

                ITextSnapshotLine line = snapshot.GetLineFromPosition(expressionStart);
                var lineText = line.GetText();
                var indentLength = lineText.Length - lineText.TrimStart().Length;
                var indent = lineText.Substring(0, indentLength);

                var transformedText = template.GetTransformedText(expressionText, indent);

                using (ITextEdit edit = buffer.CreateEdit())
                {
                    var spanToReplace = new Span(expressionStart, endPosition - expressionStart);
                    edit.Delete(spanToReplace);
                    edit.Insert(expressionStart, transformedText);
                    edit.Apply();
                }

                var placeholder = template.SelectionPlaceholder;

                if (placeholder != null)
                {
                    ITextSnapshot currentSnapshot = buffer.CurrentSnapshot;
                    var placeholderIndex = transformedText.IndexOf(placeholder, StringComparison.Ordinal);

                    if (placeholderIndex >= 0)
                    {
                        var selectionStart = expressionStart + placeholderIndex;
                        var selectionSpan = new SnapshotSpan(currentSnapshot, selectionStart, placeholder.Length);
                        _textView.Selection.Select(selectionSpan, isReversed: false);
                        _textView.Caret.MoveTo(selectionSpan.End);
                    }
                }

                return CommitResult.Handled;
            }
            catch (Exception ex)
            {
                ex.LogAsync().FireAndForget();
                return CommitResult.Unhandled;
            }
        }
    }
}
