using System;
using System.Windows;
using System.Windows.Forms;
using EyeRest.Model;
using EyeRest.Properties;
using EyeRest.Views;

namespace EyeRest
{
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _trayIcon;
        private ToolStripMenuItem _pauseItem;
        private Timer _timer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Language.SetCulture(Settings.Default.Language);

            ShowTrayIcon();

            _timer = new Timer();
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

            _timer.Dispose();
            _timer = null;
        }

        /// <summary>
        /// Shows tray icon.
        /// </summary>
        private void ShowTrayIcon()
        {
            _trayIcon = new NotifyIcon
            {
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = EyeRest.Properties.Resources.TrayIconTooltipTitle,
                BalloonTipText = EyeRest.Properties.Resources.TrayIconTooltipText,
                Text = EyeRest.Properties.Resources.TrayIconTooltipTitle,
                Icon = EyeRest.Properties.Resources.MainIcon
            };
            _trayIcon.ShowBalloonTip(1000);

            _pauseItem = new ToolStripMenuItem(EyeRest.Properties.Resources.TrayMenuSuspend, null, OnSuspendResume) { Tag = true };
            var settingsItem = new ToolStripMenuItem(EyeRest.Properties.Resources.TrayMenuShowSettings, null, OnShowSettings);
            var exitItem = new ToolStripMenuItem(EyeRest.Properties.Resources.TrayMenuExitText, null, OnExit);

            var trayMenu = new ContextMenuStrip { ShowImageMargin = false, ShowCheckMargin = false };

            trayMenu.Items.Add(settingsItem);
            trayMenu.Items.Add(_pauseItem);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add(exitItem);
            _trayIcon.ContextMenuStrip = trayMenu;

            _trayIcon.MouseUp += OnTrayIconClick;

            _trayIcon.Visible = true;

            void SetTrayMenuTexts()
            {
                _trayIcon.BalloonTipTitle = EyeRest.Properties.Resources.TrayIconTooltipTitle;
                _trayIcon.BalloonTipText = EyeRest.Properties.Resources.TrayIconTooltipText;
                _trayIcon.Text = EyeRest.Properties.Resources.TrayIconTooltipTitle;

                _pauseItem.Text = _pauseItem.Tag is true
                    ? EyeRest.Properties.Resources.TrayMenuSuspend
                    : EyeRest.Properties.Resources.TrayMenuResume;

                settingsItem.Text = EyeRest.Properties.Resources.TrayMenuShowSettings;

                exitItem.Text = EyeRest.Properties.Resources.TrayMenuExitText;
            }

            SetTrayMenuTexts();

            Language.OnChange += (object sender, EventArgs e) => { SetTrayMenuTexts(); };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_trayIcon != null)
                _trayIcon.Visible = false;
        }

        /// <summary>
        /// Handles click on 'Exit' tray icon's menu item.
        /// </summary>
        private static void OnExit(object sender, EventArgs args)
        {
            Current.Shutdown();
        }

        /// <summary>
        /// Handles click on 'Settings' tray icon's menu item.
        /// </summary>
        private static void OnShowSettings(object sender, EventArgs args)
        {
            new SettingsWindow().Show();
        }

        /// <summary>
        /// Handles click on 'Pause' tray icon's menu item.
        /// </summary>
        private void OnSuspendResume(object sender, EventArgs args)
        {
            bool state = (bool)_pauseItem.Tag;

            if (state)
            {
                _pauseItem.Text = EyeRest.Properties.Resources.TrayMenuResume;

                _timer.Suspend();
            }
            else
            {
                _pauseItem.Text = EyeRest.Properties.Resources.TrayMenuSuspend;

                _timer.Resume();
            }

            _pauseItem.Tag = !state;
        }

        /// <summary>
        /// Handles click on tray icon to open window with settings.
        /// </summary>
        private static void OnTrayIconClick(object sender, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            { OnShowSettings(sender, args); }
        }
    }
}
