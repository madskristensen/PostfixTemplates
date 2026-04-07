using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace PostfixTemplates.Completion
{
    [Export(typeof(IAsyncCompletionCommitManagerProvider))]
    [ContentType("CSharp")]
    [Name("PostfixTemplateCommitManager")]
    internal sealed class PostfixCompletionCommitManagerProvider : IAsyncCompletionCommitManagerProvider
    {
        public IAsyncCompletionCommitManager GetOrCreate(ITextView textView)
        {
            return textView.Properties.GetOrCreateSingletonProperty(() => new PostfixCompletionCommitManager(textView));
        }
    }
}
