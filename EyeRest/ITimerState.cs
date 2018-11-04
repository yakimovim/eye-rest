using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using EyeRest.ViewModels;

namespace EyeRest
{
    interface ITimerState : IDisposable
    {
        int SecondsToEnd { get; }
        void NextSecond();
        void Suspend();
        void Resume();
        event Action<ITimerState> NewStateGenerated;
    }

    abstract class TimerState : ITimerState, INotifyPropertyChanged
    {
        public TimerState(TimeSpan duration)
        {
            if (duration.TotalSeconds < 1)
            { throw new ArgumentOutOfRangeException("duration"); }

            SecondsToEnd = (int)duration.TotalSeconds;
        }

        public int SecondsToEnd { get; protected set; }

        public void NextSecond()
        {
            DecreaseSecondsToEnd();
        }

        protected virtual void DecreaseSecondsToEnd()
        {
            SecondsToEnd--;
            OnPropertyChanged("SecondsToEnd");

            if (SecondsToEnd == 0)
            {
                OnEnd();
            }
        }

        protected abstract void OnEnd();

        public event Action<ITimerState> NewStateGenerated;

        protected virtual void OnNewStateGenerated(ITimerState newState)
        {
            var handler = NewStateGenerated;
            if (handler == null)
            { return; }

            foreach (Action<ITimerState> dlgt in handler.GetInvocationList())
            {
                try { dlgt(newState); }
                catch { }
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Occurs on change of some property.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null)
            { return; }

            foreach (PropertyChangedEventHandler dlgt in handler.GetInvocationList())
            {
                try { dlgt(this, new PropertyChangedEventArgs(propertyName)); }
                catch { }
            }
        }


        public virtual void Suspend()
        {}

        public void Resume()
        {}
    }

    class WorkState : TimerState
    {
        public WorkState(TimeSpan duration)
            : base(duration)
        { }

        protected override void OnEnd()
        {
            var newState = new RestState(EyeRest.Properties.Settings.Default.RestTime);

            OnNewStateGenerated(newState);
        }
    }

    class RestState : TimerState
    {
        private RestWindow[] m_Windows;

        public int TotalSeconds { get; private set; }

        public int Seconds { get; private set; }

        /// <summary>
        /// Gets command for Cancel button.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(delegate
                {
                    OnEnd();
                });
            }
        }

        public RestState(TimeSpan duration)
            : base(duration)
        {
            TotalSeconds = SecondsToEnd;

            m_Windows = GetRestWindows();

            foreach (var window in m_Windows)
            {
                window.Closed += OnRestWindowClosed;
                window.DataContext = this;
                window.Show();
            }
        }

        private RestWindow[] GetRestWindows()
        {
            return Screen.AllScreens.Select(GetRestWindow).ToArray();
        }

        private RestWindow GetRestWindow(Screen screen)
        {
            var screenWidth = screen.WorkingArea.Width;
            var screenHeight = screen.WorkingArea.Height;

            var window = new RestWindow
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
            };

            window.Left = screen.WorkingArea.X + (screenWidth - window.Width) / 2;
            window.Top = screen.WorkingArea.Y + (screenHeight - window.Height) / 2;

            return window;
        }

        protected void OnRestWindowClosed(object sender, EventArgs e)
        {
            OnEnd();
        }

        protected override void DecreaseSecondsToEnd()
        {
            Seconds++;
            OnPropertyChanged("Seconds");

            base.DecreaseSecondsToEnd();
        }

        protected override void OnEnd()
        {
            CloseAllRestWindows();

            var newState = new WorkState(EyeRest.Properties.Settings.Default.WorkTime);

            OnNewStateGenerated(newState);
        }

        private void CloseAllRestWindows()
        {
            foreach (var window in m_Windows)
            {
                window.Closed -= OnRestWindowClosed;
                window.Close();
            }
        }

        public override void Suspend()
        {
            OnEnd();
        }
    }
}
