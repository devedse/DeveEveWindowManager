using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DeveEveWindowManager.Services
{
    public static class WindowServiceInterop
    {
        // Delegate for EnumWindows callback
        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // Import necessary functions from user32.dll
        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags
        );

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Constants for GetWindowLong
        internal const int GWL_STYLE = -16;
        internal const int WS_CAPTION = 0x00C00000; // WS_BORDER | WS_DLGFRAME

        // Constants for SetWindowPos
        internal const uint SWP_NOZORDER = 0x0004;
        internal const uint SWP_SHOWWINDOW = 0x0040;

        // Constants for ShowWindow
        internal const int SW_MAXIMIZE = 3;
        internal const int SW_RESTORE = 9;

        // Window placement states
        internal const int SW_SHOWMAXIMIZED = 3;

        public static bool HasTitleBar(IntPtr hWnd)
        {
            int style = GetWindowLong(hWnd, GWL_STYLE);
            return (style & WS_CAPTION) == WS_CAPTION;
        }

        public static bool IsWindowMaximized(IntPtr hWnd)
        {
            if (GetWindowPlacement(hWnd, out WINDOWPLACEMENT placement))
            {
                return placement.showCmd == SW_SHOWMAXIMIZED;
            }
            return false;
        }

        // Struct for GetWindowPlacement
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
