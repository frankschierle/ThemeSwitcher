namespace ThemeSwitcher
{
  using System;
  using System.ComponentModel.Design;
  using Microsoft.VisualStudio.Shell;
  using Microsoft.VisualStudio.Shell.Interop;

  using ThemeSwitcher.Logic;

  /// <summary>Command handler to switch theme and window layout.</summary>
  internal sealed class SwitchThemeAndWindowLayoutCommand
  {
    #region Constants

    /// <summary>The command id.</summary>
    private const int CommandId = 0x0100;

    #endregion

    #region Static Fields

    /// <summary>The command menu group (command set GUID).</summary>
    private static readonly Guid CommandSet = new Guid("c98bea8a-94eb-4dea-8d12-234f3d938d6d");

    #endregion

    #region Fields

    /// <summary>The VS package that provides this command.</summary>
    private readonly ThemeSwitcherPackage package;

    #endregion

    #region Constructors and Destructors

    /// <summary> Initializes a new instance of the <see cref="SwitchThemeAndWindowLayoutCommand" /> class.</summary>
    /// <param name="package">Owner package.</param>
    /// <param name="menuCommandService">The service to manage the menu commands.</param>
    /// <exception cref="ArgumentNullException">Occurs if any of the parameters is null.</exception>
    private SwitchThemeAndWindowLayoutCommand(ThemeSwitcherPackage package, OleMenuCommandService menuCommandService)
    {
      if (package == null)
      {
        throw new ArgumentNullException(nameof(package));
      }

      if (menuCommandService == null)
      {
        throw new ArgumentNullException(nameof(menuCommandService));
      }

      this.package = package;

      menuCommandService.AddCommand(new MenuCommand(this.MenuItemCallback, new CommandID(CommandSet, CommandId)));
    }

    #endregion

    #region Enums

    /// <summary>Enumeration of applied themes.</summary>
    private enum AppliedTheme
    {
      /// <summary>The first theme was applied.</summary>
      Theme1,

      /// <summary>The second theme was applied.</summary>
      Theme2
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the instance of the command.</summary>
    public static SwitchThemeAndWindowLayoutCommand Instance { get; private set; }

    #endregion

    #region Properties

    /// <summary>Gets the service provider from the owner package.</summary>
    private IServiceProvider ServiceProvider
    {
      get { return this.package; }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Initializes the singleton instance of the command.</summary>
    /// <param name="package">Owner package, not null.</param>
    /// <returns>A task representing the async work of command initialization.</returns>
    public static async System.Threading.Tasks.Task InitializeAsync(ThemeSwitcherPackage package)
    {
      OleMenuCommandService menuCommandService;

      // Switch to the main thread - the call to AddCommand in Command1's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

#pragma warning disable VSSDK006 // Check services exist
      menuCommandService = (OleMenuCommandService)await package.GetServiceAsync(typeof(IMenuCommandService)).ConfigureAwait(continueOnCapturedContext: false);
#pragma warning restore VSSDK006 // Check services exist

      Instance = new SwitchThemeAndWindowLayoutCommand(package, menuCommandService);
    }

    #endregion

    #region Methods

    /// <summary>This function is the callback used to execute the command when
    /// the menu item is clicked.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event args.</param>
    private void MenuItemCallback(object sender, EventArgs e)
    {
      AppliedTheme appliedTheme;

      try
      {
        appliedTheme = this.SwitchTheme();
        this.ApplyWindowLayoutForTheme(appliedTheme);
      }
      catch (Exception ex)
      {
        VsShellUtilities.ShowMessageBox(
          this.ServiceProvider,
          "An error occured: " + ex.Message,
          "Theme Switcher",
          OLEMSGICON.OLEMSGICON_CRITICAL,
          OLEMSGBUTTON.OLEMSGBUTTON_OK,
          OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
      }
    }

    /// <summary>Switches the theme.</summary>
    /// <returns>The applied theme.</returns>
    private AppliedTheme SwitchTheme()
    {
      var themeManager = new ThemeManager();
      Theme currentTheme = themeManager.GetCurrentTheme();
      Theme targetTheme = null;
      AppliedTheme appliedTheme;

      if ((this.package.Options.Theme1Id == null) || (this.package.Options.Theme2Id == null))
      {
        throw new InvalidOperationException("Themes not configured. Please check Theme Switcher settings.");
      }

      if (currentTheme == null)
      {
        throw new InvalidOperationException("The current theme cannot be determined.");
      }

      if (currentTheme.Id.Equals(this.package.Options.Theme1Id))
      {
        // First theme is applied => switch to theme 2.
        targetTheme = themeManager.GetThemeById(this.package.Options.Theme2Id);
        appliedTheme = AppliedTheme.Theme2;
      }
      else
      {
        // Second theme or a manually applied theme is active => switch to theme 1
        targetTheme = themeManager.GetThemeById(this.package.Options.Theme1Id);
        appliedTheme = AppliedTheme.Theme1;
      }

      if (targetTheme == null)
      {
        throw new InvalidOperationException("Target theme not found. Please check Theme Switcher settings.");
      }

      themeManager.ApplyTheme(targetTheme);

      return appliedTheme;
    }

    /// <summary>Applies the window layout for an applied theme.</summary>
    /// <param name="theme">The theme that was applied.</param>
    private void ApplyWindowLayoutForTheme(AppliedTheme theme)
    {
      string targetLayoutKey = theme == AppliedTheme.Theme1 ? this.package.Options.WindowLayout1Key : this.package.Options.WindowLayout2Key;
      var windowLayoutManager = new WindowLayoutManager();
      WindowLayout targetLayout;

      if (targetLayoutKey != string.Empty)
      {
        targetLayout = windowLayoutManager.GetWindowLayoutByKey(targetLayoutKey);

        if (targetLayout == null)
        {
          throw new InvalidOperationException("Target window layout not found. Please check Theme Switcher settings.");
        }

        windowLayoutManager.ApplyWindowLayout(targetLayout);
      }
    }

    #endregion
  }
}
