namespace ThemeSwitcher.Options
{
  using System;
  using System.Windows.Forms;

  /// <summary>The user control shown in the Visual Studio options dialog to
  /// display the Theme Switcher options.</summary>
  public partial class ThemeSwitcherOptionsControl : UserControl
  {
    #region Fields

    /// <summary>The options page where all options are stored.</summary>
    private ThemeSwitcherOptionsDialogPage optionsPage;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instnce of the <see cref="ThemeSwitcherOptionsControl" /> class.</summary>
    /// <param name="optionsPage">The options page that should be used by the control.</param>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="optionsPage" /> is null.</exception>
    public ThemeSwitcherOptionsControl(ThemeSwitcherOptionsDialogPage optionsPage)
    {
      if (optionsPage == null)
      {
        throw new ArgumentNullException(nameof(optionsPage));
      }

      this.optionsPage = optionsPage;

      this.InitializeComponent();
    }

    #endregion
  }
}
