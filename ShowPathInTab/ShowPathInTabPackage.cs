﻿using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using System.Reflection;
using Microsoft.VisualStudio.PlatformUI.Shell;

namespace ShowPathInTab
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(ShowPathInTabPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class ShowPathInTabPackage : AsyncPackage
    {
        /// <summary>
        /// ShowPathInTabPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "7f0fbee7-3f0a-4050-9b3c-a782e141ff33";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            MergeResources();

            ShowPathInTabViewElementFactory.SetupExistingDocuments();

            
            ViewElementFactory.Current = new ShowPathInTabViewElementFactory(ViewElementFactory.Current);
        }

        private static void MergeResources()
        {
            var resourceDictionary = LoadResourceDictionary("Views/DataTemplates.xaml");
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private static ResourceDictionary LoadResourceDictionary(string xamlName)
        {
            return (ResourceDictionary)Application.LoadComponent(
                new Uri(Assembly.GetExecutingAssembly().GetName().Name + ";component/" + xamlName, UriKind.Relative));
        }

        #endregion
    }
}
