using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Windows.Win32;

namespace Sungaila.ImmersiveDarkMode.WinForms
{
    [SupportedOSPlatform("windows10.0.22000.0")]
    public static class WindowExtensions
    {
        /// <summary>
        /// Checks if the given message broadcasts a change to the system-wide application theme. Then applies the titlebar theme accordingly.
        /// This method should be called within <see cref="Form.WndProc(ref Message)"/>;
        /// </summary>
        /// <param name="m">The message to check.</param>
        public static void CheckAppsThemeChanged(Message m)
        {
            if (m.Msg != PInvoke.WM_SETTINGCHANGE || m.WParam != IntPtr.Zero || m.LParam == IntPtr.Zero || Marshal.PtrToStringUni(m.LParam) != "ImmersiveColorSet")
                return;

            NativeMethods.SetTitlebarTheme(m.HWnd);
        }

        /// <summary>
        /// Sets the titlebar theme according to Windows system-wide application theme.
        /// </summary>
        /// <param name="window">The Windows Forms window for which the titlebar theme is set.</param>
        public static void SetTitlebarTheme(this Form form)
        {
            ArgumentNullException.ThrowIfNull(form);

            SetTitlebarTheme(form, NativeMethods.GetAppsUseLightTheme());
        }

        /// <summary>
        /// Sets the titlebar theme according to Windows system-wide application theme.
        /// </summary>
        /// <param name="window">The Windows Forms window for which the titlebar theme is set.</param>
        /// <param name="isLightTheme">Determines whether the light or dark theme is applied.</param>
        public static void SetTitlebarTheme(this Form form, bool isLightTheme)
        {
            ArgumentNullException.ThrowIfNull(form);

            NativeMethods.SetTitlebarTheme(form.Handle, isLightTheme);
        }
    }
}