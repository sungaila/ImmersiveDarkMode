using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

namespace Sungaila.ImmersiveDarkMode
{
    [SupportedOSPlatform("windows10.0.22000.0")]
    /// <summary>
    /// Provides methods for setting the titlebar theme in Windows.
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// Reads the Windows registry to check if applications use the light theme.
        /// Defaults to light theme if the registry value was not found.
        /// </summary>
        /// <returns>Returns <see langword="true"/> for light and <see langword="false"/> for dark theme.</returns>
        public static bool GetAppsUseLightTheme()
        {
            using var currentUserKey = Registry.CurrentUser;
            using var subKey = currentUserKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

            if (subKey?.GetValue("AppsUseLightTheme") is not int value)
                return true;

            return value != 0x00000000;
        }

        /// <summary>
        /// Sets the titlebar theme according to the current system-wide application theme.
        /// </summary>
        /// <param name="hwnd">The handle to the window for which the titlebar theme is set.</param>
        public static void SetTitlebarTheme(nint hwnd)
        {
            SetTitlebarTheme(hwnd, GetAppsUseLightTheme());
        }

        /// <summary>
        /// Sets the titlebar theme.
        /// </summary>
        /// <param name="hwnd">The handle to the window for which the titlebar theme is set.</param>
        /// <param name="isLightTheme">Determines whether the light or dark theme is applied.</param>
        public static void SetTitlebarTheme(nint hwnd, bool isLightTheme)
        {
            int attrValue = isLightTheme ? 0 : 1;

            DwmSetWindowAttribute(
                hwnd,
                DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                attrValue,
                sizeof(int));
        }

        internal unsafe static void DwmSetWindowAttribute(nint hwnd, DWMWINDOWATTRIBUTE attr, int attrValue, uint attrSize)
        {
            var handle = GCHandle.Alloc(attrValue, GCHandleType.Pinned);

            try
            {
                PInvoke.DwmSetWindowAttribute(
                    (HWND)hwnd,
                    attr,
                    handle.AddrOfPinnedObject().ToPointer(),
                    attrSize);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}