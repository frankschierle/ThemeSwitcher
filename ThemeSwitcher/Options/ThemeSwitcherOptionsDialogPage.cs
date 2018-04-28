namespace ThemeSwitcher.Options
{
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Runtime.CompilerServices;
  using System.Runtime.InteropServices;
  using System.Windows;
  using System.Windows.Data;

  using Microsoft.VisualStudio.Shell;

  using ThemeSwitcher.Logic;

  /// <summary>The options dialog page of the Theme Switcher extension.</summary>
  [Guid("0A6251BA-38BC-4ED7-BDF6-81096F6C5D9E")]
  public class ThemeSwitcherOptionsDialogPage : UIElementDialogPage,
                                                IThemeSwitcherOptions,
                                                INotifyPropertyChanged
  {
    #region Fields

    /// <summary>See <see cref="Theme1Id" />.</summary>
    private string theme1Id;

    /// <summary>See <see cref="Theme2Id" />.</summary>
    private string theme2Id;

    /// <summary>See <see cref="WindowLayout1Key" />.</summary>
    private string windowLayout1Key;

    /// <summary>See <see cref="WindowLayout2Key" />.</summary>
    private string windowLayout2Key;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ThemeSwitcherOptionsDialogPage" /> class.</summary>
    public ThemeSwitcherOptionsDialogPage()
    {
      var themeManager = new ThemeManager();
      var windowLayoutManager = new WindowLayoutManager();
      var themes1 = new ObservableCollection<Theme>(themeManager.GetInstalledThemes());
      var themes2 = new ObservableCollection<Theme>(themeManager.GetInstalledThemes());
      var windowLayouts = new ObservableCollection<WindowLayout>(windowLayoutManager.GetWindowLayouts());

      windowLayouts.Add(new WindowLayout(string.Empty, -1, "Do not change window layout"));

      this.AvailableThemes1 = CollectionViewSource.GetDefaultView(themes1);
      this.AvailableThemes1.SortDescriptions.Add(new SortDescription(nameof(Theme.DisplayName), ListSortDirection.Ascending));
      this.AvailableThemes1.Filter = theme => !((Theme)theme).Id.Equals(this.Theme2Id);

      this.AvailableThemes2 = CollectionViewSource.GetDefaultView(themes2);
      this.AvailableThemes2.SortDescriptions.Add(new SortDescription(nameof(Theme.DisplayName), ListSortDirection.Ascending));
      this.AvailableThemes2.Filter = theme => !((Theme)theme).Id.Equals(this.Theme1Id);

      this.AvailableWindowLayouts = CollectionViewSource.GetDefaultView(windowLayouts);
      this.AvailableWindowLayouts.SortDescriptions.Add(new SortDescription(nameof(WindowLayout.Index), ListSortDirection.Ascending));
    }

    #endregion

    #region Public Events

    /// <inheritdoc />
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Properties

    /// <summary>Provides a collection of all available themes for <see cref="Theme1Id" />.</summary>
    public ICollectionView AvailableThemes1 { get; }

    /// <summary>Provides a collection of all available themes for <see cref="Theme2Id" />.</summary>
    public ICollectionView AvailableThemes2 { get; }

    /// <inheritdoc />
    public string Theme1Id
    {
      get { return this.theme1Id; }
      set
      {
        if (value != this.theme1Id)
        {
          this.theme1Id = value;
          this.OnPropertyChanged();

          ((CollectionView)this.AvailableThemes2).Refresh();
        }
      }
    }

    /// <inheritdoc />
    public string Theme2Id
    {
      get { return this.theme2Id; }
      set
      {
        if (value != this.theme2Id)
        {
          this.theme2Id = value;
          this.OnPropertyChanged();

          ((CollectionView)this.AvailableThemes1).Refresh();
        }
      }
    }

    /// <summary>Provides a collection of all available window layouts.</summary>
    public ICollectionView AvailableWindowLayouts { get; }

    /// <inheritdoc />
    public string WindowLayout1Key
    {
      get { return this.windowLayout1Key; }
      set
      {
        if (value != this.windowLayout1Key)
        {
          this.windowLayout1Key = value;
          this.OnPropertyChanged();
        }
      }
    }

    /// <inheritdoc />
    public string WindowLayout2Key
    {
      get { return this.windowLayout2Key; }
      set
      {
        if (value != this.windowLayout2Key)
        {
          this.windowLayout2Key = value;
          this.OnPropertyChanged();
        }
      }
    }

    #endregion

    #region Properties

    /// <summary>Gets the WPF child element to be hosted inside the Options dialog page.</summary>
    /// <returns>The WPF child element.</returns>
    protected override UIElement Child
    {
      get { return new ThemeSwitcherOptionsControl(this); }
    }

    #endregion

    #region Methods

    /// <summary>Raises the <see cref="PropertyChanged" /> event.</summary>
    /// <param name="propertyName">The name of the property.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
