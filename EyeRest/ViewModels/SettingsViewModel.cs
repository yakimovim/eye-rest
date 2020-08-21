using System;
using System.Diagnostics;
using System.Windows.Input;
using EyeRest.Model;
using EyeRest.Views;

namespace EyeRest.ViewModels
{
    class SettingsViewModel : ViewModel<SettingsWindow>
    {
        private int _workTimeMinutes;
        private int _workTimeSeconds;
        private int _restTimeMinutes;
        private int _restTimeSeconds;
        private int _languageIndex;

        public SettingsViewModel(SettingsWindow window) : base(window)
        {
            var settings = Properties.Settings.Default;

            _workTimeMinutes = settings.WorkTime.Minutes;
            _workTimeSeconds = settings.WorkTime.Seconds;
            _restTimeMinutes = settings.RestTime.Minutes;
            _restTimeSeconds = settings.RestTime.Seconds;

            _languageIndex = settings.Language;
        }

        /// <summary>
        /// Gets or sets work time minutes.
        /// </summary>
        public int WorkTimeMinutes
        {
            [DebuggerStepThrough]
            get { return _workTimeMinutes; }
            [DebuggerStepThrough]
            set
            {
                if (_workTimeMinutes != value)
                {
                    _workTimeMinutes = value;

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
            get { return _workTimeSeconds; }
            [DebuggerStepThrough]
            set
            {
                if (_workTimeSeconds != value)
                {
                    _workTimeSeconds = value;

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
            get { return _restTimeMinutes; }
            [DebuggerStepThrough]
            set
            {
                if (_restTimeMinutes != value)
                {
                    _restTimeMinutes = value;

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
            get { return _restTimeSeconds; }
            [DebuggerStepThrough]
            set
            {
                if (_restTimeSeconds != value)
                {
                    _restTimeSeconds = value;

                    OnPropertyChanged("RestTimeSeconds");
                }
            }
        }

        /// <summary>
        /// Gets or sets index of interface language.
        /// </summary>
        public int LanguageIndex
        {
            [DebuggerStepThrough]
            get { return _languageIndex; }
            [DebuggerStepThrough]
            set
            {
                if (_languageIndex != value)
                {
                    _languageIndex = value;

                    Language.SetCulture(value);

                    OnPropertyChanged("LanguageIndex");
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

                    settings.Language = LanguageIndex;

                    settings.Save();

                    Language.SetCulture(LanguageIndex);

                    _control.Close();
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
                    var settings = Properties.Settings.Default;

                    Language.SetCulture(settings.Language);

                    _control.Close();
                });
            }
        }

    }
}
