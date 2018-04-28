namespace ThemeSwitcher
{
  using System.Runtime.InteropServices;

  using Microsoft.VisualStudio.Shell;

  using ThemeSwitcher.Options;

  /// <summary>This is the class that implements the ThemeSwitcher Visual Studio package.</summary>
  [Guid(PackageGuidString)]
  [PackageRegistration(UseManagedResourcesOnly = true)]
  [ProvideOptionPage(typeof(ThemeSwitcherOptionsDialogPage), "Theme Switcher", "General", 0, 0, true)]
  [InstalledProductRegistration("Theme Switcher", "A Visual Studio extension that allows users to fast switch between themes and window layouts.", "1.0")]
  [ProvideMenuResource("Menus.ctmenu", 1)]
  public sealed class ThemeSwitcherPackage : Package
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

    /// <summary>Initialization of the package; this method is called right
    /// after the package is sited, so this is the place where you can put all
    /// the initialization code that rely on services provided by VisualStudio.
    /// </summary>
    protected override void Initialize()
    {
      base.Initialize();
      SwitchThemeAndWindowLayoutCommand.Initialize(this);
    }

    #endregion
  }
}
