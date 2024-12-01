using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DeveEveWindowManager.Services;
using DeveEveWindowManager.ViewModels;
using DeveEveWindowManager.Views;
using Microsoft.Extensions.DependencyInjection;
using DeveEveWindowManager.AvaloniaHelpers;

namespace DeveEveWindowManager
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Register all the services needed for the application to run
            var collection = new ServiceCollection();
            collection.AddCommonServices();

            // Creates a ServiceProvider containing services from the provided IServiceCollection
            var services = collection.BuildServiceProvider();

            var vm = services.GetRequiredService<MainWindowViewModel>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainWindow
                {
                    DataContext = vm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
