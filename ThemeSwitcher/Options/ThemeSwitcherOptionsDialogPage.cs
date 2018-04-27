namespace ThemeSwitcher.Options
{
  using System.Runtime.InteropServices;
  using System.Windows.Forms;

  using Microsoft.VisualStudio.Shell;

  /// <summary>The options dialog page of the Theme Switcher extension.</summary>
  [Guid("0A6251BA-38BC-4ED7-BDF6-81096F6C5D9E")]
  public class ThemeSwitcherOptionsDialogPage : UIElementDialogPage {
    #region Properties

    /// <summary>Gets the WPF child element to be hosted inside the Options dialog page.</summary>
    /// <returns>The WPF child element.</returns>
    protected override System.Windows.UIElement Child {
      get { return new ThemeSwitcherOptionsControl(this); }
    }

    #endregion
  }
}
