using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sungaila.ImmersiveDarkMode.WinForms
{
    public class ImmersiveColorSetMessageFilter(nint Hwnd) : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == Windows.Win32.PInvoke.WM_SETTINGCHANGE && m.WParam == IntPtr.Zero && m.LParam != IntPtr.Zero && Marshal.PtrToStringUni(m.LParam) == "ImmersiveColorSet")
            {
                NativeMethods.SetTitlebarTheme(Hwnd);
            }

            return false;
        }
    }
}