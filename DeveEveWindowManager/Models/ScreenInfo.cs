using Avalonia;

namespace DeveEveWindowManager.Models
{
    public class ScreenInfo
    {
        public PixelRect OriginalBounds { get; set; }
        public Rect RelativeBounds { get; set; }
        public bool IsPrimary { get; set; }


        public PixelRect WorkingArea { get; set; }
        public double Scaling { get; set; }

        public int Size => WorkingArea.Width;
        public int TopX => WorkingArea.X;
        public int TopY => WorkingArea.Y;
    }
}
