using System;

namespace DeveEveWindowManager.Models
{
    public class WindowInstance
    {
        public required string WindowTitle { get; set; }
        public IntPtr HWnd { get; set; }
        public required bool HasTitleBar { get; set; }
    }
}
