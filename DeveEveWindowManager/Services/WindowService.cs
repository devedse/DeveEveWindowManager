using Avalonia.Threading;
using DeveEveWindowManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveEveWindowManager.Services
{
    public class WindowService
    {
        // Delegate for EnumWindows callback
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // Import necessary functions from user32.dll
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags
        );

        // Constants for SetWindowPos
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_SHOWWINDOW = 0x0040;

        public WindowService() { }

        public List<WindowInstance> GetEveWindows()
        {
            var foundWindows = new List<WindowInstance>();

            // Enumerate all top-level windows
            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                if (IsWindowVisible(hWnd))
                {
                    StringBuilder sb = new StringBuilder(256);
                    GetWindowText(hWnd, sb, sb.Capacity);
                    string title = sb.ToString();
                    if (title.IndexOf("EVE - ", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundWindows.Add(new WindowInstance() { WindowTitle = title, HWnd = hWnd });
                    }
                }
                return true;
            }, IntPtr.Zero);

            return foundWindows;
        }

    }
}
