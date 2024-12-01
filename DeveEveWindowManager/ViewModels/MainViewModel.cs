using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveEveWindowManager.Models;
using DeveEveWindowManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DeveEveWindowManager.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to DeveEveWindowManager, please click on Reload screens first";
    private readonly ScreenService _screenService;

    public ObservableCollection<ScreenInfo> Screens { get; }

    public ICommand LoadScreensCommand => new RelayCommand(LoadScreens);

    public double RelativeWidthComparedToHeight => (double)Screens.Max(t => t.OriginalBounds.X + t.OriginalBounds.Width) / Screens.Max(t => t.OriginalBounds.Y + t.OriginalBounds.Height);

    public MainViewModel()
    {
        //Design time
        Screens = new ObservableCollection<ScreenInfo>(MockScreens());

    }

    public MainViewModel(ScreenService screenService)
    {
        Screens = new ObservableCollection<ScreenInfo>(MockScreens());
        _screenService = screenService;
    }


    private void LoadScreens()
    {
        Screens.Clear();
        foreach (var screen in _screenService.GetScreens())
        {
            Screens.Add(screen);
        }
    }

    private List<ScreenInfo> MockScreens()
    {
        return new List<ScreenInfo>()
            {
                new ScreenInfo()
                {
                    OriginalBounds = new PixelRect(0, 0, 2560, 1440),
                    RelativeBounds = new Rect(0, 0, 0.42, 1),
                    IsPrimary = true,
                    WorkingArea = new PixelRect(0, 0, 100, 100),
                    Scaling = 1,
                    DisplayName = "G27Q"
                },
                new ScreenInfo()
                {
                    OriginalBounds = new PixelRect(2560, 0, 3440, 1440),
                    RelativeBounds = new Rect(0.42, 0, 0.58, 1),
                    IsPrimary = false,
                    WorkingArea = new PixelRect(150, 0, 100, 200),
                    Scaling = 1,
                    DisplayName = "Dell AW3418DW"
                }
            };
    }

}
