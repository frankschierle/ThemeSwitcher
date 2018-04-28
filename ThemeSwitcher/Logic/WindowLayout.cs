namespace ThemeSwitcher.Logic
{
  using System.Diagnostics;

  /// <summary>Represents a window layout.</summary>
  [DebuggerDisplay("{DisplayName} ({Index})")]
  public class WindowLayout
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="WindowLayout" /> class.</summary>
    /// <param name="key">The key of the window layout.</param>
    /// <param name="index">The index of the window layout.</param>
    /// <param name="displayName">The display name of the window layout.</param>
    public WindowLayout(string key, int index, string displayName)
    {
      this.Key = key;
      this.Index = index;
      this.DisplayName = displayName;
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the key of the window layout.</summary>
    public string Key { get; }

    /// <summary>Gets the index of the window layout.</summary>
    public int Index { get; }

    /// <summary>Gets the display name of the window layout.</summary>
    public string DisplayName { get; }

    #endregion
  }
}
