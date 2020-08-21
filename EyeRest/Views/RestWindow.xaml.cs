using System.Media;
using System.Windows;
using System.Windows.Input;

namespace EyeRest.Views
{
    /// <summary>
    /// Interaction logic for RestWindow.xaml
    /// </summary>
    public partial class RestWindow : Window
    {
        public RestWindow()
        {
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            SystemSounds.Asterisk.Play();
        }
    }
}
