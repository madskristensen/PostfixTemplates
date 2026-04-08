global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using PostfixTemplates.Templates;

namespace PostfixTemplates
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), Vsix.Name, "General", 0, 0, true, SupportsProfiles = true)]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(PackageGuids.PostfixTemplatesString)]
    public sealed class PostfixTemplatesPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            VS.Events.SolutionEvents.OnAfterOpenSolution += OnAfterOpenSolution;
            VS.Events.SolutionEvents.OnAfterCloseSolution += OnAfterCloseSolution;

            // If a solution is already open when the package loads, initialize immediately
            Solution solution = await VS.Solutions.GetCurrentSolutionAsync();
            var solutionPath = solution?.FullPath;

            if (!string.IsNullOrEmpty(solutionPath))
            {
                var solutionDir = Path.GetDirectoryName(solutionPath);
                CustomTemplateLoader.Initialize(solutionDir);
            }
        }

        private void OnAfterOpenSolution(Solution solution)
        {
            var solutionPath = solution?.FullPath;

            if (!string.IsNullOrEmpty(solutionPath))
            {
                var solutionDir = Path.GetDirectoryName(solutionPath);
                CustomTemplateLoader.Initialize(solutionDir);
            }
        }

        private void OnAfterCloseSolution()
        {
            CustomTemplateLoader.Clear();
        }
    }
}