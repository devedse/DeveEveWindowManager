using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DeveEveWindowManager.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow MainWindowDingFirstTimeOnlyForReadingScreens;

        public MainWindow()
        {
            MainWindowDingFirstTimeOnlyForReadingScreens = this;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
