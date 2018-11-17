namespace ThemeSwitcher.Options
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
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
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ICollectionView AvailableThemes1 { get; }

    /// <summary>Provides a collection of all available themes for <see cref="Theme2Id" />.</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

    #region Public Methods and Operators

    /// <inheritdoc />
    public override void SaveSettingsToStorage()
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(
        this.AutomationObject,
        new Attribute[]
        {
          DesignerSerializationVisibilityAttribute.Visible
        });

      foreach (object obj in properties)
      {
        this.SaveSetting((PropertyDescriptor)obj);
      }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void OnActivate(CancelEventArgs e)
    {
      var themeManager = new ThemeManager();
      var windowLayoutManager = new WindowLayoutManager();
      IEnumerable<Theme> themes = themeManager.GetInstalledThemes();
      IEnumerable<WindowLayout> windowLayouts = windowLayoutManager.GetWindowLayouts();
      string currentTheme1 = this.Theme1Id;
      string currentTheme2 = this.Theme2Id;
      string currentLayout1 = this.WindowLayout1Key;
      string currentLayout2 = this.WindowLayout2Key;

      windowLayouts = windowLayouts.Union(new[]
                                          {
                                            new WindowLayout(string.Empty, -1, "Do not change window layout")
                                          });


      this.UpdateCollectionView(this.AvailableThemes1, themes);
      this.UpdateCollectionView(this.AvailableThemes2, themes);
      this.UpdateCollectionView(this.AvailableWindowLayouts, windowLayouts);

      this.Theme1Id = currentTheme1;
      this.Theme2Id = currentTheme2;
      this.WindowLayout1Key = currentLayout1;
      this.WindowLayout2Key = currentLayout2;

      base.OnActivate(e);
    }

    /// <summary>Raises the <see cref="PropertyChanged" /> event.</summary>
    /// <param name="propertyName">The name of the property.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>Updates a <see cref="ICollectionView" />.</summary>
    /// <typeparam name="T">The type of the objects included in the CollectionViews source collection.</typeparam>
    /// <param name="collectionView">The CollectionView to update.</param>
    /// <param name="items">All items that should be included in the CollectionView.</param>
    private void UpdateCollectionView<T>(ICollectionView collectionView, IEnumerable<T> items)
    {
      var targetCollection = (ObservableCollection<T>)collectionView.SourceCollection;
      IEnumerable<T> itemsToAdd = items.Where(i => !targetCollection.Any(t => t.Equals(i)));
      IEnumerable<T> itemsToRemove = targetCollection.Where(t => !items.Any(i => i.Equals(t)));

      foreach (T i in itemsToAdd.ToArray())
      {
        targetCollection.Add(i);
      }

      foreach (T i in itemsToRemove.ToArray())
      {
        targetCollection.Remove(i);
      }
    }

    #endregion
  }
}
