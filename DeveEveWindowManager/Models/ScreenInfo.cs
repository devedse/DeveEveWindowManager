using Avalonia;
using System;

namespace DeveEveWindowManager.Models
{
    public class ScreenInfo
    {
        public PixelRect OriginalBounds { get; set; }
        public Rect RelativeBounds { get; set; }
        public bool IsPrimary { get; set; }
        public string? DisplayName { get; set; }

        public PixelRect WorkingArea { get; set; }
        public double Scaling { get; set; }

        public string ScreenDetails => $"{DisplayName}{Environment.NewLine}({OriginalBounds.Width}x{OriginalBounds.Height})";
    }
}
