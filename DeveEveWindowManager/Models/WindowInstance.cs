using System;
using System.Collections.Generic;

namespace DeveEveWindowManager.Models
{
    public class WindowInstance
    {
        public required string WindowTitle { get; set; }
        public IntPtr HWnd { get; set; }     
    }
}
