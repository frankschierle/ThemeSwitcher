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

    #region Public Methods and Operators

    /// <summary>Determines whether the specified <see cref="WindowLayout" /> object is
    /// equal to the current object.</summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object;
    /// otherwise, false.</returns>
    public bool Equals(WindowLayout other)
    {
      bool equals = false;

      if (other != null)
      {
        equals = this.Key.Equals(other.Key);
        equals &= this.Index.Equals(other.Index);
        equals &= this.DisplayName.Equals(other.DisplayName);
      }

      return equals;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != this.GetType())
      {
        return false;
      }

      return this.Equals(obj as WindowLayout);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        int hashCode = 13;

        hashCode = (hashCode * 397) ^ this.Key.GetHashCode();
        hashCode = (hashCode * 397) ^ this.Index.GetHashCode();
        hashCode = (hashCode * 397) ^ this.DisplayName.GetHashCode();

        return hashCode;
      }
    }

    #endregion
  }
}
