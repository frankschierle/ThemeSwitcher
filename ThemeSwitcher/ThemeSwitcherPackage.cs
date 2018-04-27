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
  public sealed class ThemeSwitcherPackage : Package
  {
    #region Constants

    /// <summary>Gets the ThemeSwitcherPackage GUID string.</summary>
    public const string PackageGuidString = "49a61599-600e-49ec-a363-7009c2a66a4f";

    #endregion
  }
}
