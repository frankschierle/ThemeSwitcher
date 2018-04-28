namespace ThemeSwitcher
{
  using System;
  using System.ComponentModel.Design;
  using System.Globalization;

  using Microsoft.VisualStudio.Shell;
  using Microsoft.VisualStudio.Shell.Interop;

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
    private readonly Package package;

    #endregion

    #region Constructors and Destructors

    /// <summary> Initializes a new instance of the <see cref="SwitchThemeAndWindowLayoutCommand" /> class.</summary>
    /// <param name="package">Owner package.</param>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="package" /> is null.</exception>
    private SwitchThemeAndWindowLayoutCommand(Package package)
    {
      OleMenuCommandService commandService;

      if (package == null)
      {
        throw new ArgumentNullException(nameof(package));
      }

      this.package = package;

      commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

      if (commandService != null)
      {
        commandService.AddCommand(new MenuCommand(this.MenuItemCallback, new CommandID(CommandSet, CommandId)));
      }
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
    public static void Initialize(Package package)
    {
      Instance = new SwitchThemeAndWindowLayoutCommand(package);
    }

    #endregion

    #region Methods

    /// <summary>This function is the callback used to execute the command when the menu item is clicked.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event args.</param>
    private void MenuItemCallback(object sender, EventArgs e)
    {
      string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
      string title = "SwitchThemeAndWindowLayoutCommand";

      // Show a message box to prove we were here
      VsShellUtilities.ShowMessageBox(
        this.ServiceProvider,
        message,
        title,
        OLEMSGICON.OLEMSGICON_INFO,
        OLEMSGBUTTON.OLEMSGBUTTON_OK,
        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    #endregion
  }
}
