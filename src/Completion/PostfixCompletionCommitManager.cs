using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ITextView _textView;

        public PostfixCompletionCommitManager(ITextView textView)
        {
            _textView = textView;
        }

        public IEnumerable<char> PotentialCommitCharacters
        {
            get
            {
                return new[] { '\t', '\n' };
            }
        }

        public bool ShouldCommitCompletion(IAsyncCompletionSession session, SnapshotPoint location, char typedChar, CancellationToken cancellationToken)
        {
            var selectedItem = session.GetComputedItems(cancellationToken).SelectedItem;

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

                var template = PostfixTemplate.All.FirstOrDefault(t => t.Name == templateName);

                if (template == null)
                {
                    return CommitResult.Unhandled;
                }

                var snapshot = buffer.CurrentSnapshot;
                var applicableSpan = session.ApplicableToSpan.GetSpan(snapshot);
                var endPosition = applicableSpan.End.Position;

                var line = snapshot.GetLineFromPosition(expressionStart);
                var lineText = line.GetText();
                var indentLength = lineText.Length - lineText.TrimStart().Length;
                var indent = lineText.Substring(0, indentLength);

                var transformedText = template.GetTransformedText(expressionText, indent);

                using (var edit = buffer.CreateEdit())
                {
                    var spanToReplace = new Span(expressionStart, endPosition - expressionStart);
                    edit.Delete(spanToReplace);
                    edit.Insert(expressionStart, transformedText);
                    edit.Apply();
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
