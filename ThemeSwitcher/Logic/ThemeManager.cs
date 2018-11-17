namespace ThemeSwitcher.Logic
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

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

    /// <summary>Gets the current Visual Studio Theme.</summary>
    /// <returns>The current Visual Studio theme or null if the current theme cannot be determined.</returns>
    public Theme GetCurrentTheme()
    {
      string keyName = string.Format(CultureInfo.InvariantCulture, @"{0}\ApplicationPrivateSettings\Microsoft\VisualStudio", this.Dte.RegistryRoot);
      IEnumerable<Theme> allThemes = this.GetInstalledThemes();
      Theme result = null;
      string storedSetting;
      string[] settings;
      string id;

      using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
      {
        if (key != null)
        {
          storedSetting = (string)key.GetValue("ColorTheme", string.Empty);

          if (!string.IsNullOrEmpty(storedSetting))
          {
            settings = storedSetting.Split('*');

            if (settings.Length > 2)
            {
              id = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", settings[2]);
              result = allThemes.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            }
          }
        }
      }

      return result;
    }

    /// <summary>Gets a theme by its id.</summary>
    /// <param name="id">The id of the theme.</param>
    /// <returns>The theme with the given <paramref name="id" /> or null if the theme does not exist.</returns>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="id" /> is null.</exception>
    public Theme GetThemeById(string id)
    {
      IEnumerable<Theme> allThemes;

      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }

      allThemes = this.GetInstalledThemes();

      return allThemes.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Applies a given <see cref="Theme" />.</summary>
    /// <param name="theme">The theme to apply.</param>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="theme" /> is null.</exception>
    public void ApplyTheme(Theme theme)
    {
      string keyName = string.Format(CultureInfo.InvariantCulture, @"{0}\ApplicationPrivateSettings\Microsoft\VisualStudio", this.Dte.RegistryRoot);

      if (theme == null)
      {
        throw new ArgumentNullException(nameof(theme));
      }

      using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
      {
        if (key != null)
        {
          key.SetValue("ColorTheme", "0*System.String*" + theme.Id.Trim('{', '}'));
          key.SetValue("ColorThemeNew", "0*System.String*" + theme.Id);
        }
      }

      NativeMethods.SendNotifyMessage(
        new IntPtr(NativeMethods.HWND_BROADCAST),
        NativeMethods.WM_SYSCOLORCHANGE,
        IntPtr.Zero,
        IntPtr.Zero);
    }

    #endregion
  }
}
