using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Interop;
using Windows.Win32;

namespace Sungaila.ImmersiveDarkMode.Wpf
{
    [SupportedOSPlatform("windows10.0.22000.0")]
    public static class WindowExtensions
    {
        /// <summary>
        /// Set the titlebar theme whenever Windows system-wide application theme is changed.
        /// </summary>
        /// <param name="window">The WPF window for which the titlebar theme is set.</param>
        public static void InitTitlebarTheme(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            HwndSource? hwndSource = null;

            void sourceInitializedHandler(object? sender, EventArgs e)
            {
                window.SourceInitialized -= sourceInitializedHandler;
                hwndSource = AddWndProcHook(new WindowInteropHelper(window).Handle);
                SetTitlebarTheme(window);
            }

            void closedHandler(object? sender, EventArgs e)
            {
                window.Closed -= closedHandler;
                hwndSource?.Dispose();
            }

            if (new WindowInteropHelper(window).Handle == IntPtr.Zero)
            {
                window.SourceInitialized -= sourceInitializedHandler;
                window.SourceInitialized += sourceInitializedHandler;
            }
            else
            {
                sourceInitializedHandler(null, EventArgs.Empty);
            }

            window.Closed -= closedHandler;
            window.Closed += closedHandler;
        }

        private static HwndSource AddWndProcHook(IntPtr hwnd)
        {
            var source = HwndSource.FromHwnd(hwnd);
            source.AddHook(new HwndSourceHook(WndProc));

            return source;
        }

        private static nint WndProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            try
            {
                if (msg == PInvoke.WM_SETTINGCHANGE && wParam == 0 && lParam != 0 && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet")
                {
                    NativeMethods.SetTitlebarTheme(hwnd);
                    handled = true;
                }
            }
            catch (Exception)
            {
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Sets the titlebar theme according to Windows system-wide application theme.
        /// </summary>
        /// <param name="window">The WPF window for which the titlebar theme is set.</param>
        public static void SetTitlebarTheme(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            SetTitlebarTheme(window, NativeMethods.GetAppsUseLightTheme());
        }

        /// <summary>
        /// Sets the titlebar theme according to Windows system-wide application theme.
        /// </summary>
        /// <param name="window">The WPF window for which the titlebar theme is set.</param>
        /// <param name="isLightTheme">Determines whether the light or dark theme is applied.</param>
        public static void SetTitlebarTheme(this Window window, bool isLightTheme)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            NativeMethods.SetTitlebarTheme(new WindowInteropHelper(window).Handle, isLightTheme);
        }
    }
}