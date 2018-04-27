namespace ThemeSwitcher.Logic
{
  using System.Collections.Generic;

  using EnvDTE;

  using Microsoft.VisualStudio.Shell;
  using Microsoft.Win32;

  /// <summary>Provides methods to manage installed Visual Studio themes.</summary>
  internal class ThemeManager
  {
    #region Properties

    /// <summary>Gets the DTE automation object.</summary>
    private DTE Dte
    {
      get { return (DTE)Package.GetGlobalService(typeof(DTE)); }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Gets all themes installed in Visual Studio.</summary>
    /// <returns>All themes installed in Visual Studio.</returns>
    public IEnumerable<Theme> GetInstalledThemes()
    {
      string registryPath = this.Dte.RegistryRoot + "_Config\\Themes";
      var themes = new List<Theme>();
      string[] installedThemesKeys;

      using (RegistryKey themesKey = Registry.CurrentUser.OpenSubKey(registryPath))
      {
        if (themesKey != null)
        {
          installedThemesKeys = themesKey.GetSubKeyNames();

          foreach (string key in installedThemesKeys)
          {
            using (RegistryKey themeKey = themesKey.OpenSubKey(key))
            {
              if (themeKey != null)
              {
                themes.Add(new Theme(key, themeKey.GetValue(null).ToString()));
              }
            }
          }
        }
      }

      return themes;
    }

    #endregion
  }
}
