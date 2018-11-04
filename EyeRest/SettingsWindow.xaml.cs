using System.Windows;
using EyeRest.ViewModels;

namespace EyeRest
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            var viewModel = new SettingsViewModel(this);
            this.DataContext = viewModel;
        }
    }
}
