using Microsoft.UI.Xaml;
using System;
using System.Runtime.Versioning;
using WinRT.Interop;

namespace Sungaila.ImmersiveDarkMode.WinUI
{
    [SupportedOSPlatform("windows10.0.22000.0")]
    public static class WindowExtensions
    {
        /// <summary>
        /// Set the titlebar theme whenever <see cref="FrameworkElement.ActualThemeChanged"/> is raised.
        /// </summary>
        /// <param name="window">The WinUI window for which the titlebar theme is set.</param>
        public static void InitTitlebarTheme(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            if (window.Content is not FrameworkElement frameworkElement)
                return;

            void actualThemeChangedHandler(FrameworkElement sender, object args) => SetTitlebarTheme(window);

            frameworkElement.ActualThemeChanged -= actualThemeChangedHandler;
            frameworkElement.ActualThemeChanged += actualThemeChangedHandler;

            void closedHandler(object sender, WindowEventArgs args)
            {
                frameworkElement.ActualThemeChanged -= actualThemeChangedHandler;
                window.Closed -= closedHandler;
            };

            window.Closed -= closedHandler;
            window.Closed += closedHandler;

            SetTitlebarTheme(window);
        }

        /// <summary>
        /// Sets the titlebar theme according to <see cref="Application.Current.RequestedTheme"/>.
        /// </summary>
        /// <param name="window">The WinUI window for which the titlebar theme is set.</param>
        public static void SetTitlebarTheme(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            SetTitlebarTheme(window, Application.Current.RequestedTheme == ApplicationTheme.Light);
        }

        /// <summary>
        /// Sets the titlebar theme.
        /// </summary>
        /// <param name="window">The WinUI window for which the titlebar theme is set.</param>
        /// <param name="isLightTheme">Determines whether the light or dark theme is applied.</param>
        public static void SetTitlebarTheme(this Window window, bool isLightTheme)
        {
            ArgumentNullException.ThrowIfNull(window);

            NativeMethods.SetTitlebarTheme(WindowNative.GetWindowHandle(window), isLightTheme);
        }
    }
}