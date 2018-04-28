namespace ThemeSwitcher.Logic
{
  using System.Diagnostics;

  /// <summary>Represents a Visual Studio theme.</summary>
  [DebuggerDisplay("{DisplayName}")]
  public class Theme
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
    /// <param name="id">The id of the theme.</param>
    /// <param name="displayName">The display name of the theme.</param>
    public Theme(string id, string displayName)
    {
      this.Id = id;
      this.DisplayName = displayName;
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the id of the theme.</summary>
    public string Id { get; }

    /// <summary>Gets the display name of the theme.</summary>
    public string DisplayName { get; }

    #endregion
  }
}
