using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace DeveEveWindowManager.Models
{
    public partial class ScreenInfo : ObservableObject
    {
        public PixelRect OriginalBounds { get; set; }
        public Rect RelativeBounds { get; set; }
        public bool IsPrimary { get; set; }
        public string? DisplayName { get; set; }

        public PixelRect WorkingArea { get; set; }
        public double Scaling { get; set; }

        public string ScreenDetails => $"{DisplayName}{Environment.NewLine}({OriginalBounds.Width}x{OriginalBounds.Height})";

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private double _idealCameraCenter;

        public ICommand? ToggleSelectionCommand => new RelayCommand(() =>
        {
            IsSelected = !IsSelected;
        });
    }
}
