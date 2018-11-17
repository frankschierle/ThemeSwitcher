namespace ThemeSwitcher
{
  using System;
  using System.Runtime.InteropServices;
  using System.Threading;
  using Microsoft.VisualStudio.Shell;

  using ThemeSwitcher.Options;

  /// <summary>This is the class that implements the ThemeSwitcher Visual Studio package.</summary>
  [Guid(PackageGuidString)]
  [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
  [ProvideOptionPage(typeof(ThemeSwitcherOptionsDialogPage), "Theme Switcher", "General", 0, 0, true)]
  [ProvideProfile(typeof(ThemeSwitcherOptionsDialogPage), "Theme Switcher", "General", 100, 100, false, DescriptionResourceID = 100)]
  [InstalledProductRegistration("Theme Switcher", "A Visual Studio extension that allows users to fast switch between themes and window layouts.", "1.1")]
  [ProvideMenuResource("Menus.ctmenu", 1)]
  public sealed class ThemeSwitcherPackage : AsyncPackage
  {
    #region Constants

    /// <summary>Gets the ThemeSwitcherPackage GUID string.</summary>
    public const string PackageGuidString = "49a61599-600e-49ec-a363-7009c2a66a4f";

    #endregion

    #region Properties

    /// <summary>Gets all extension options.</summary>
    internal IThemeSwitcherOptions Options
    {
      get
      {
        var optionsPage = (ThemeSwitcherOptionsDialogPage)this.GetDialogPage(typeof(ThemeSwitcherOptionsDialogPage));

        optionsPage.LoadSettingsFromStorage();

        return optionsPage;
      }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
      await base.InitializeAsync(cancellationToken, progress).ConfigureAwait(continueOnCapturedContext: false);

      await SwitchThemeAndWindowLayoutCommand.InitializeAsync(this).ConfigureAwait(continueOnCapturedContext: false);
    }

    #endregion
  }
}
