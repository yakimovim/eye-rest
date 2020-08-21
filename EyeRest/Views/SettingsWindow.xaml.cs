using System.Windows;
using EyeRest.ViewModels;

namespace EyeRest.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            DataContext = new SettingsViewModel(this);
        }
    }
}
