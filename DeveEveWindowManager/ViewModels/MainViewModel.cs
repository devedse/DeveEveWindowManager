using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using DeveEveWindowManager.Models;
using DeveEveWindowManager.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DeveEveWindowManager.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    public ObservableCollection<ScreenInfo> Screens { get; }

    private List<ScreenInfo> MockScreens()
    {
        //return new List<ScreenInfo>()
        //{
        //    new ScreenInfo()
        //    {
        //        OriginalBounds = new PixelRect(0, 0, 2560, 1440),
        //        RelativeBounds = new Rect(0, 0, 0.42, 1),
        //        Primary = true,
        //        WorkingArea = new PixelRect(0, 0, 1920, 1080),
        //        PixelDensity = 1
        //    },
        //    new ScreenInfo()
        //    {
        //        OriginalBounds = new PixelRect(0, 0, 3440, 1440),
        //        RelativeBounds = new Rect(0.42, 0, 0.58, 1),
        //        Primary = true,
        //        WorkingArea = new PixelRect(0, 0, 1920, 1080),
        //        PixelDensity = 1
        //    }
        //};
        return new List<ScreenInfo>()
            {
                new ScreenInfo()
                {
                    OriginalBounds = new PixelRect(0, 0, 2560, 1440),
                    RelativeBounds = new Rect(0, 0, 0.42, 1),
                    IsPrimary = true,
                    WorkingArea = new PixelRect(0, 0, 100, 100),
                    Scaling = 1
                },
                new ScreenInfo()
                {
                    OriginalBounds = new PixelRect(2560, 0, 3440, 1440),
                    RelativeBounds = new Rect(0.42, 0, 0.58, 1),
                    IsPrimary = false,
                    WorkingArea = new PixelRect(150, 0, 100, 200),
                    Scaling = 1
                }
            };
    }

    public MainViewModel()
    {
        //Design time
        Screens = new ObservableCollection<ScreenInfo>(MockScreens());

    }

    public MainViewModel(ScreenService screenService)
    {
        Screens = new ObservableCollection<ScreenInfo>(MockScreens());
    }
}
