namespace ThemeSwitcher
{
  using System.Runtime.InteropServices;

  using Microsoft.VisualStudio.Shell;

  /// <summary>This is the class that implements the ThemeSwitcher Visual Studio package.</summary>
  [PackageRegistration(UseManagedResourcesOnly = true)]
  [InstalledProductRegistration("Theme Switcher", "A Visual Studio extension that allows users to fast switch between themes and window layouts.", "1.0")]
  [Guid(PackageGuidString)]
  public sealed class ThemeSwitcherPackage : Package
  {
    #region Constants

    /// <summary>Gets the ThemeSwitcherPackage GUID string.</summary>
    public const string PackageGuidString = "49a61599-600e-49ec-a363-7009c2a66a4f";

    #endregion
  }
}
