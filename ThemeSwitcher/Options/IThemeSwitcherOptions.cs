namespace ThemeSwitcher.Options
{
  /// <summary>Provides acces to all options for the extension.</summary>
  internal interface IThemeSwitcherOptions
  {
    #region Public Properties

    /// <summary>Gets the id of the first theme.</summary>
    string Theme1Id { get; }

    /// <summary>Gets the id of the second theme.</summary>
    string Theme2Id { get; }

    /// <summary>Gets the key of the first window layout.</summary>
    string WindowLayout1Key { get; }

    /// <summary>Gets the key of the second window layout.</summary>
    string WindowLayout2Key { get; }

    #endregion
  }
}
