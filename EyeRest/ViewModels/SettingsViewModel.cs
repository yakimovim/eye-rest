using System;
using System.Diagnostics;
using System.Windows.Input;

namespace EyeRest.ViewModels
{
    class SettingsViewModel : ViewModel<SettingsWindow>
    {
        private int m_WorkTimeMinutes;
        private int m_WorkTimeSeconds;
        private int m_RestTimeMinutes;
        private int m_RestTimeSeconds;

        public SettingsViewModel(SettingsWindow window) : base(window)
        {
            var settings = Properties.Settings.Default;

            m_WorkTimeMinutes = settings.WorkTime.Minutes;
            m_WorkTimeSeconds = settings.WorkTime.Seconds;
            m_RestTimeMinutes = settings.RestTime.Minutes;
            m_RestTimeSeconds = settings.RestTime.Seconds;
        }

        /// <summary>
        /// Gets or sets work time minutes.
        /// </summary>
        public int WorkTimeMinutes
        {
            [DebuggerStepThrough]
            get { return m_WorkTimeMinutes; }
            [DebuggerStepThrough]
            set
            {
                if (m_WorkTimeMinutes != value)
                {
                    m_WorkTimeMinutes = value;

                    OnPropertyChanged("WorkTimeMinutes");
                }
            }
        }

        /// <summary>
        /// Gets or sets work time seconds.
        /// </summary>
        public int WorkTimeSeconds
        {
            [DebuggerStepThrough]
            get { return m_WorkTimeSeconds; }
            [DebuggerStepThrough]
            set
            {
                if (m_WorkTimeSeconds != value)
                {
                    m_WorkTimeSeconds = value;

                    OnPropertyChanged("WorkTimeSeconds");
                }
            }
        }
        /// <summary>
        /// Gets or sets rest time minutes.
        /// </summary>
        public int RestTimeMinutes
        {
            [DebuggerStepThrough]
            get { return m_RestTimeMinutes; }
            [DebuggerStepThrough]
            set
            {
                if (m_RestTimeMinutes != value)
                {
                    m_RestTimeMinutes = value;

                    OnPropertyChanged("RestTimeMinutes");
                }
            }
        }

        /// <summary>
        /// Gets or sets rest time seconds.
        /// </summary>
        public int RestTimeSeconds
        {
            [DebuggerStepThrough]
            get { return m_RestTimeSeconds; }
            [DebuggerStepThrough]
            set
            {
                if (m_RestTimeSeconds != value)
                {
                    m_RestTimeSeconds = value;

                    OnPropertyChanged("RestTimeSeconds");
                }
            }
        }

        /// <summary>
        /// Gets command for Ok button.
        /// </summary>
        public ICommand OkCommand
        {
            get
            {
                return new RelayCommand(delegate
                {
                    var settings = Properties.Settings.Default;

                    settings.WorkTime = new TimeSpan(0, WorkTimeMinutes, WorkTimeSeconds);
                    settings.RestTime = new TimeSpan(0, RestTimeMinutes, RestTimeSeconds);

                    settings.Save();

                    m_Control.Close();
                });
            }
        }

        /// <summary>
        /// Gets command for Cancel button.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(delegate
                {
                    m_Control.Close();
                });
            }
        }

    }
}
