using DeveEveWindowManager.Services;
using DeveEveWindowManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DeveEveWindowManager.AvaloniaHelpers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<ScreenService>();
            collection.AddSingleton<WindowService>();

            collection.AddTransient<MainViewModel>();
        }
    }
}
