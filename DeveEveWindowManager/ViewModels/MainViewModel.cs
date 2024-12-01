using Avalonia;
using Avalonia.Threading;
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
    private string _greeting = "Welcome to DeveEveWindowManager, please send any donations to Devedse (ingame)";
    private readonly ScreenService? _screenService;
    private readonly WindowService? _windowService;

    public ObservableCollection<ScreenInfo> Screens { get; } = new();
    public ObservableCollection<WindowInstance> EveInstances { get; } = new();

    public ICommand LoadScreensCommand => new RelayCommand(LoadScreens);
    public ICommand LoadWindowInstancesCommand => new RelayCommand(LoadWindowInstances);
    [ObservableProperty]
    private WindowInstance? _selectedEveInstance;

    public ICommand ApplyCommand => new RelayCommand(() => Apply());

    public double RelativeWidthComparedToHeight => Screens.Count == 0 ? 1 : (double)Screens.Max(t => t.OriginalBounds.X + t.OriginalBounds.Width) / Screens.Max(t => t.OriginalBounds.Y + t.OriginalBounds.Height);

    private DispatcherTimer _disTimer = new DispatcherTimer();

    public MainViewModel()
    {
        //Design time
        foreach (var screen in MockScreens())
        {
            Screens.Add(screen);
        }
        EveInstances.Add(new WindowInstance() { WindowTitle = "EVE - Devedse" });
        EveInstances.Add(new WindowInstance() { WindowTitle = "EVE - AnotherCharacter" });
    }

    public MainViewModel(ScreenService screenService, WindowService windowService)
    {
        _disTimer.Interval = TimeSpan.FromSeconds(0);
        _disTimer.Tick += (sender, e) => LoadAll();
        _disTimer.Start();

        Screens.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(RelativeWidthComparedToHeight));

        _screenService = screenService;
        _windowService = windowService;
    }

    private void LoadAll()
    {
        LoadScreens();
        LoadWindowInstances();
        _disTimer.Interval = TimeSpan.FromSeconds(5);
    }

    private void LoadScreens()
    {
        Screens.Clear();
        foreach (var screen in _screenService?.GetScreens() ?? [])
        {
            Screens.Add(screen);
        }
    }

    private void LoadWindowInstances()
    {
        EveInstances.Clear();
        foreach (var windowInstance in _windowService?.GetEveWindows() ?? [])
        {
            EveInstances.Add(windowInstance);
        }
    }

    private void Apply()
    {
        if (SelectedEveInstance == null)
        {
            return;
        }

        var selectedScreens = Screens.Where(t => t.IsSelected).ToList();

        _windowService?.MoveAndResizeWindow(SelectedEveInstance, selectedScreens);
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
