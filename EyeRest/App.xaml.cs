using System;
using System.Windows;
using System.Windows.Forms;

namespace EyeRest
{
    public partial class App : System.Windows.Application
    {
        private NotifyIcon m_TrayIcon;
        private ToolStripMenuItem m_PauseItem;
        private Timer m_Timer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShowTrayIcon();

            m_Timer = new Timer();
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

            m_Timer.Dispose();
            m_Timer = null;
        }

        /// <summary>
        /// Shows tray icon.
        /// </summary>
        private void ShowTrayIcon()
        {
            m_TrayIcon = new NotifyIcon
            {
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = EyeRest.Properties.Resources.TrayIconTooltipTitle,
                BalloonTipText = EyeRest.Properties.Resources.TrayIconTooltipText,
                Text = EyeRest.Properties.Resources.TrayIconTooltipTitle,
                Icon = EyeRest.Properties.Resources.MainIcon
            };
            m_TrayIcon.ShowBalloonTip(1000);

            m_PauseItem = new ToolStripMenuItem(EyeRest.Properties.Resources.TrayMenuSuspend, null, OnSuspendResume) { Tag = true };

            var trayMenu = new ContextMenuStrip { ShowImageMargin = false, ShowCheckMargin = false };
            trayMenu.Items.Add(EyeRest.Properties.Resources.TrayMenuShowSettings, null, OnShowSettings);
            trayMenu.Items.Add(m_PauseItem);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add(EyeRest.Properties.Resources.TrayMenuExitText, null, OnExit);
            m_TrayIcon.ContextMenuStrip = trayMenu;

            m_TrayIcon.MouseUp += OnTrayIconClick;

            m_TrayIcon.Visible = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (m_TrayIcon != null)
                m_TrayIcon.Visible = false;
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
            bool state = (bool)m_PauseItem.Tag;

            if (state)
            {
                m_PauseItem.Text = EyeRest.Properties.Resources.TrayMenuResume;

                m_Timer.Suspend();
            }
            else
            {
                m_PauseItem.Text = EyeRest.Properties.Resources.TrayMenuSuspend;

                m_Timer.Resume();
            }

            m_PauseItem.Tag = !state;
        }

        /// <summary>
        /// Handles click on tray icon to open window with settings.
        /// </summary>
        private static void OnTrayIconClick(object sender, System.Windows.Forms.MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            { OnShowSettings(sender, args); }
        }
    }
}
