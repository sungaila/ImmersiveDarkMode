using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace Sungaila.ImmersiveDarkMode.WinForms
{
    [SupportedOSPlatform("windows10.0.22000.0")]
    public static class WindowExtensions
    {
        /// <summary>
        /// Set the titlebar theme whenever Windows system-wide application theme is changed.
        /// </summary>
        /// <param name="window">The Windows Forms window for which the titlebar theme is set.</param>
        public static void InitTitlebarTheme(this Form form)
        {
            ArgumentNullException.ThrowIfNull(form);

            var filter = new ImmersiveColorSetMessageFilter(form.Handle);
            Application.AddMessageFilter(filter);

            void handleCreated(object? sender, EventArgs e)
            {
                form.HandleCreated -= handleCreated;
                SetTitlebarTheme(form);
            }

            void formClosed(object? sender, FormClosedEventArgs e)
            {
                form.HandleCreated -= handleCreated;
                form.FormClosed -= formClosed;
                Application.RemoveMessageFilter(filter);
            }

            form.FormClosed -= formClosed;
            form.FormClosed += formClosed;

            if (!form.IsHandleCreated)
            {
                form.HandleCreated -= handleCreated;
                form.HandleCreated += handleCreated;
            }
            else
            {
                handleCreated(null, EventArgs.Empty);
            }
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