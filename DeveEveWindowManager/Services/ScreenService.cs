using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DeveEveWindowManager.Models;
using DeveEveWindowManager.Views;
using System;
using System.Collections.Generic;

namespace DeveEveWindowManager.Services
{
    public class ScreenService
    {
        public ScreenService()
        {
        }

        public IEnumerable<ScreenInfo> GetScreens()
        {
            var blah = (IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;
            var mainWindow = blah.MainWindow;

            var mainWindow2 = MainWindow.MainWindowDingFirstTimeOnlyForReadingScreens;

            var screens = new List<ScreenInfo>();
            var screenImpl = mainWindow.Screens;

            // First, determine the overall bounds encompassing all screens
            double minX = double.MaxValue, minY = double.MaxValue;
            double maxX = double.MinValue, maxY = double.MinValue;

            foreach (var screen in screenImpl.All)
            {
                var bounds = screen.Bounds;
                minX = Math.Min(minX, bounds.X);
                minY = Math.Min(minY, bounds.Y);
                maxX = Math.Max(maxX, bounds.X + bounds.Width);
                maxY = Math.Max(maxY, bounds.Y + bounds.Height);
            }

            double totalWidth = maxX - minX;
            double totalHeight = maxY - minY;

            // Avoid division by zero
            if (totalWidth == 0 || totalHeight == 0)
            {
                totalWidth = totalWidth == 0 ? 1 : totalWidth;
                totalHeight = totalHeight == 0 ? 1 : totalHeight;
            }

            // Now, calculate the relative bounds for each screen
            foreach (var screen in screenImpl.All)
            {
                var bounds = screen.Bounds;

                double relativeX = (bounds.X - minX) / totalWidth;
                double relativeY = (bounds.Y - minY) / totalHeight;
                double relativeWidth = bounds.Width / totalWidth;
                double relativeHeight = bounds.Height / totalHeight;

                var relativeBounds = new Rect(relativeX, relativeY, relativeWidth, relativeHeight);

                screens.Add(new ScreenInfo
                {
                    OriginalBounds = bounds,
                    RelativeBounds = relativeBounds,
                    Primary = screen.Primary,
                    WorkingArea = screen.WorkingArea,
                    PixelDensity = screen.PixelDensity
                });
            }

            return screens;
        }
    }
}
