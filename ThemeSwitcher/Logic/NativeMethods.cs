namespace ThemeSwitcher.Logic
{
  using System;
  using System.Runtime.InteropServices;

  /// <summary>Provides access to native Win32 functions.</summary>
  internal static class NativeMethods
  {
    /// <summary>Broadcast to all windows.</summary>
    public const int HWND_BROADCAST = 0xFFFF;

    /// <summary>The WM_SYSCOLORCHANGE message is sent to all top-level windows
    /// when a change is made to a system color setting.</summary>
    public const int WM_SYSCOLORCHANGE = 0x15;

    /// <summary>Sends the specified message to a window or windows.</summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="Msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
  }
}
