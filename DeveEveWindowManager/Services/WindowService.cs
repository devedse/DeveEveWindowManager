using DeveEveWindowManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveEveWindowManager.Services
{
    public class WindowService
    {
        public WindowService() { }

        public List<WindowInstance> GetEveWindows()
        {
            var foundWindows = new List<WindowInstance>();

            // Enumerate all top-level windows
            WindowServiceInterop.EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                if (WindowServiceInterop.IsWindowVisible(hWnd))
                {
                    StringBuilder sb = new StringBuilder(256);
                    WindowServiceInterop.GetWindowText(hWnd, sb, sb.Capacity);
                    string title = sb.ToString();
                    if (title.IndexOf("EVE - ", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        bool hasTitleBar = WindowServiceInterop.HasTitleBar(hWnd);
                        foundWindows.Add(new WindowInstance()
                        {
                            WindowTitle = title,
                            HWnd = hWnd,
                            HasTitleBar = hasTitleBar
                        });
                    }
                }
                return true;
            }, IntPtr.Zero);

            return foundWindows;
        }

        public void MoveAndResizeWindow(WindowInstance selectedWindow, List<ScreenInfo> desiredScreens)
        {
            if (selectedWindow == null)
            {
                Console.WriteLine("No window selected.");
                return;
            }

            if (desiredScreens == null || !desiredScreens.Any())
            {
                Console.WriteLine("No screens selected.");
                return;
            }

            // Unmaximize the window if it is maximized
            if (WindowServiceInterop.IsWindowMaximized(selectedWindow.HWnd))
            {
                Console.WriteLine($"Window '{selectedWindow.WindowTitle}' is maximized. Restoring it...");
                WindowServiceInterop.ShowWindow(selectedWindow.HWnd, WindowServiceInterop.SW_RESTORE);
            }

            int left = desiredScreens.Min(screen => screen.OriginalBounds.X);
            int top = desiredScreens.Min(screen => screen.OriginalBounds.Y);
            int right = desiredScreens.Max(screen => screen.OriginalBounds.Right);
            int bottom = desiredScreens.Max(screen => screen.OriginalBounds.Bottom);

            if (selectedWindow.HasTitleBar)
            {
                foreach (var screen in desiredScreens.Where(t => t.HasTaskbar))
                {
                    if (screen.OriginalBounds.Bottom != screen.WorkingArea.Bottom)
                    {
                        //Taskbar on the bottom
                        bottom = screen.WorkingArea.Bottom;
                    }
                    if (screen.OriginalBounds.Right != screen.WorkingArea.Right)
                    {
                        //Taskbar on the right
                        right = screen.WorkingArea.Right;
                    }
                    if (screen.OriginalBounds.X != screen.WorkingArea.X)
                    {
                        //Taskbar on the left
                        left = screen.WorkingArea.X;
                    }
                    if (screen.OriginalBounds.Y != screen.WorkingArea.Y)
                    {
                        //Taskbar on the top
                        top = screen.WorkingArea.Y;
                    }
                }
            }

            int width = right - left;
            int height = bottom - top;

            // Move and resize the window
            if (selectedWindow.HWnd != IntPtr.Zero)
            {
                WindowServiceInterop.SetWindowPos(
                    selectedWindow.HWnd,
                    IntPtr.Zero,
                    left,
                    top,
                    width,
                    height,
                    WindowServiceInterop.SWP_NOZORDER | WindowServiceInterop.SWP_SHOWWINDOW);
                Console.WriteLine($"Window '{selectedWindow.WindowTitle}' moved and resized to bounds: ({left}, {top}, {width}, {height})");

                if (desiredScreens.Count == 1 && selectedWindow.HasTitleBar == true)
                {
                    Console.WriteLine($"Only one screen selected. Maximizing window '{selectedWindow.WindowTitle}'...");
                    WindowServiceInterop.ShowWindow(selectedWindow.HWnd, WindowServiceInterop.SW_MAXIMIZE);
                }
            }
            else
            {
                Console.WriteLine($"Invalid window handle for '{selectedWindow.WindowTitle}'.");
            }
        }
    }
}
