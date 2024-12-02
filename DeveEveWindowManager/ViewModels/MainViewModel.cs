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
    public string AssemblyVersion => typeof(MainViewModel).Assembly.GetName().Version?.ToString() ?? "Unknown";

    [ObservableProperty]
    private string _greeting = "Welcome to DeveEveWindowManager, select your client window and the screens you want the window to show at.";
    private readonly ScreenService? _screenService;
    private readonly WindowService? _windowService;

    public ObservableCollection<ScreenInfo> Screens { get; } = new();
    public ObservableCollection<WindowInstance> EveInstances { get; } = new();

    public ICommand LoadScreensCommand => new RelayCommand(LoadScreens);
    public ICommand LoadWindowInstancesCommand => new RelayCommand(LoadWindowInstances);
    [ObservableProperty]
    private WindowInstance? _selectedEveInstance;

    public ICommand ApplyCommand => new RelayCommand(Apply);

    public double RelativeWidthComparedToHeight => Screens.Count == 0 ? 1 : (double)Screens.Max(t => t.OriginalBounds.X + t.OriginalBounds.Width) / Screens.Max(t => t.OriginalBounds.Y + t.OriginalBounds.Height);

    private DispatcherTimer _disTimer = new DispatcherTimer();

    public MainViewModel()
    {
        //Design time
        foreach (var screen in MockScreens())
        {
            Screens.Add(screen);
        }
        EveInstances.Add(new WindowInstance() { WindowTitle = "EVE - Devedse", HasTitleBar = false });
        EveInstances.Add(new WindowInstance() { WindowTitle = "EVE - AnotherCharacter", HasTitleBar = false });
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
        var previousScreens = Screens.ToList();
        previousScreens.ForEach(t => t.PropertyChanged -= (sender, e) => CalculateIdealCameraCenterPerScreen());
        Screens.Clear();
        foreach (var screen in _screenService?.GetScreens() ?? [])
        {
            var previousScreen = previousScreens.FirstOrDefault(t => t.OriginalBounds == screen.OriginalBounds);
            screen.IsSelected = previousScreen?.IsSelected ?? false;
            screen.PropertyChanged += (sender, e) => CalculateIdealCameraCenterPerScreen();
            Screens.Add(screen);
        }
        CalculateIdealCameraCenterPerScreen();
    }

    private void LoadWindowInstances()
    {
        var newEveWindows = _windowService?.GetEveWindows() ?? [];

        var toRemove = EveInstances.Where(t => !newEveWindows.Any(t2 => t2.WindowTitle == t.WindowTitle)).ToList();
        foreach (var remove in toRemove)
        {
            EveInstances.Remove(remove);
        }
        var toAdd = newEveWindows.Where(t => !EveInstances.Any(t2 => t2.WindowTitle == t.WindowTitle)).ToList();
        foreach (var add in toAdd)
        {
            EveInstances.Add(add);
        }

        var toUpdate = newEveWindows.Where(t => EveInstances.Any(t2 => t2.WindowTitle == t.WindowTitle)).ToList();
        foreach (var update in toUpdate)
        {
            var existing = EveInstances.First(t => t.WindowTitle == update.WindowTitle);
            existing.HWnd = update.HWnd;
        }

        if (SelectedEveInstance == null && EveInstances.Count > 0)
        {
            SelectedEveInstance = EveInstances[0];
        }
    }

    public void CalculateIdealCameraCenterPerScreen()
    {
        var selectedScreens = Screens.Where(t => t.IsSelected).ToList();

        if (selectedScreens.Any())
        {
            var totalViewPortRelativeStartX = selectedScreens.Min(t => t.RelativeBounds.X);
            var totalViewPortRelativeEndX = selectedScreens.Max(t => t.RelativeBounds.X + t.RelativeBounds.Width);
            var totalViewPortRelativeWidth = totalViewPortRelativeEndX - totalViewPortRelativeStartX;

            //Desired camera center is a value between -100% and +100%. So with one screen this is always 0. With 2 depending on what screen you want to center at it's going to be moved.
            foreach (var screen in selectedScreens)
            {
                var valueBetween0And1 = screen.RelativeBounds.X + screen.RelativeBounds.Width / 2;
                var valueBetween0And1ScaledToCurrentlyActiveScreens = (valueBetween0And1 - totalViewPortRelativeStartX) / totalViewPortRelativeWidth;
                screen.IdealCameraCenter = Math.Round((valueBetween0And1ScaledToCurrentlyActiveScreens - 0.5) * 200);
            }
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
                    DisplayName = "G27Q",
                    IdealCameraCenter = -60.3
                },
                new ScreenInfo()
                {
                    OriginalBounds = new PixelRect(2560, 0, 3440, 1440),
                    RelativeBounds = new Rect(0.42, 0, 0.58, 1),
                    IsPrimary = false,
                    WorkingArea = new PixelRect(150, 0, 100, 200),
                    Scaling = 1,
                    DisplayName = "Dell AW3418DW",
                    IdealCameraCenter = 43.7
                }
            };
    }
}
