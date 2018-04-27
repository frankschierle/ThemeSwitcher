namespace ThemeSwitcher.Options
{
  using System.Runtime.InteropServices;
  using System.Windows.Forms;

  using Microsoft.VisualStudio.Shell;

  /// <summary>The options dialog page of the Theme Switcher extension.</summary>
  [Guid("0A6251BA-38BC-4ED7-BDF6-81096F6C5D9E")]
  public class ThemeSwitcherOptionsDialogPage : DialogPage
  {
    #region Properties

    /// <inheritdoc />
    protected override IWin32Window Window
    {
      get { return new ThemeSwitcherOptionsControl(this); }
    }

    #endregion
  }
}
