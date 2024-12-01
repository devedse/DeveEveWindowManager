using Avalonia;

namespace DeveEveWindowManager.Models
{
    public class ScreenInfo
    {
        public PixelRect OriginalBounds { get; set; }
        public Rect RelativeBounds { get; set; }
        public bool Primary { get; set; }


        public PixelRect WorkingArea { get; set; }
        public double PixelDensity { get; set; }
    }
}
