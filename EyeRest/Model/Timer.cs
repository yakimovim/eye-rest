using System;
using System.Windows.Threading;
using EyeRest.Model;

namespace EyeRest
{
    internal class Timer : IDisposable
    {
        private DispatcherTimer _timer;
        private ITimerState _timerState;

        public Timer()
        {
            _timerState = new WorkState(Properties.Settings.Default.WorkTime);
            _timerState.NewStateGenerated += OnNewStateGenerated;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += OnEachSecond;
            _timer.Start();
        }

        public void Suspend()
        {
            _timer.IsEnabled = false;
            _timerState.Suspend();
        }

        public void Resume()
        {
            _timerState.Resume();
            _timer.IsEnabled = true;
        }

        private void OnEachSecond(object sender, EventArgs e)
        {
            _timerState.NextSecond();
        }

        private void OnNewStateGenerated(ITimerState newState)
        {
            _timerState.NewStateGenerated -= OnNewStateGenerated;
            _timerState.Dispose();

            newState.NewStateGenerated += OnNewStateGenerated;
            _timerState = newState;
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= OnEachSecond;
            _timerState.NewStateGenerated -= OnNewStateGenerated;
            _timerState.Dispose();
        }
    }
}
