using Avalonia;
using DeveEveWindowManager.Models;
using DeveEveWindowManager.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DeveEveWindowManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public ObservableCollection<ScreenInfo> Screens { get; }

        public MainWindowViewModel()
        {
            //Design time
            Screens = new ObservableCollection<ScreenInfo>();
            Screens.Add(new ScreenInfo()
            {
                OriginalBounds = new PixelRect(0, 0, 2560, 1440),
                RelativeBounds = new Rect(0, 0, 0.42, 1),
                Primary = true,
                WorkingArea = new PixelRect(0, 0, 1920, 1080),
                PixelDensity = 1
            });
            Screens.Add(new ScreenInfo()
            {
                OriginalBounds = new PixelRect(0, 0, 3440, 1440),
                RelativeBounds = new Rect(0.42, 0, 0.58, 1),
                Primary = true,
                WorkingArea = new PixelRect(0, 0, 1920, 1080),
                PixelDensity = 1
            });
        }

        public MainWindowViewModel(ScreenService screenService)
        {
            Screens = new ObservableCollection<ScreenInfo>();
            Screens.Add(new ScreenInfo()
            {
                OriginalBounds = new PixelRect(0, 0, 2560, 1440),
                RelativeBounds = new Rect(0, 0, 0.42, 1),
                Primary = true,
                WorkingArea = new PixelRect(0, 0, 1920, 1080),
                PixelDensity = 1
            });
            Screens.Add(new ScreenInfo()
            {
                OriginalBounds = new PixelRect(0, 0, 3440, 1440),
                RelativeBounds = new Rect(0.42, 0, 0.58, 1),
                Primary = true,
                WorkingArea = new PixelRect(0, 0, 1920, 1080),
                PixelDensity = 1
            });
        }

        
    }
}
