namespace ThemeSwitcher.Logic
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  using EnvDTE;

  using Microsoft.VisualStudio.Shell;
  using Microsoft.Win32;

  using Newtonsoft.Json.Linq;

  /// <summary>Provides methods to manage window layouts.</summary>
  internal class WindowLayoutManager
  {
    #region Properties

    /// <summary>Gets the DTE automation object.</summary>
    private DTE Dte
    {
      get { return (DTE)Package.GetGlobalService(typeof(DTE)); }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Gets all defined window layouts.</summary>
    /// <returns>All defined window layouts.</returns>
    public IEnumerable<WindowLayout> GetWindowLayouts()
    {
      string registryPath = this.Dte.RegistryRoot + @"\ApplicationPrivateSettings\_metadata\baselines\Microsoft\Visualstudio\Platform\WindowManagement\Layouts";
      var result = new List<WindowLayout>();
      string rawValues;
      JArray layouts;
      string displayName;
      string key;
      int index;
      string value;
      string[] values;

      using (RegistryKey k = Registry.CurrentUser.OpenSubKey(registryPath))
      {
        if (k != null)
        {
          rawValues = (string)k.GetValue("WindowLayoutInfoList", string.Empty);

          if (!string.IsNullOrEmpty(rawValues))
          {
            layouts = JArray.Parse(rawValues.Trim('1'));

            foreach (JToken layout in layouts)
            {
              key = layout["Key"].Value<string>();
              value = layout["Value"].Value<string>();
              values = value.Split('|');

              if (values.Length > 2)
              {
                index = int.Parse(values[1]);
                displayName = values[2];

                result.Add(new WindowLayout(key, index, displayName));
              }
            }
          }
        }
      }

      return result;
    }

    /// <summary>Gets a window layout by its key.</summary>
    /// <param name="key">The key of the window layout.</param>
    /// <returns>The window layout with the given key or null if no window layout with the given key exists.</returns>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="key" /> is null.</exception>
    public WindowLayout GetWindowLayoutByKey(string key)
    {
      IEnumerable<WindowLayout> allLayouts;

      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      allLayouts = this.GetWindowLayouts();

      return allLayouts.FirstOrDefault(l => l.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Applies a given <see cref="WindowLayout" />.</summary>
    /// <param name="layout">The layout to apply.</param>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="layout" /> is null.</exception>
    public void ApplyWindowLayout(WindowLayout layout)
    {
      string cmd;

      if (layout == null)
      {
        throw new ArgumentNullException(nameof(layout));
      }

      cmd = string.Format(CultureInfo.InvariantCulture, "Window.ApplyWindowLayout{0}", layout.Index + 1);
      this.Dte.ExecuteCommand(cmd);
    }

    #endregion
  }
}
