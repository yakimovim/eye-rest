using System;
using System.Windows.Threading;

namespace EyeRest
{
    internal class Timer : IDisposable
    {
        private DispatcherTimer m_Timer;
        private ITimerState m_TimerState;

        public Timer()
        {
            m_TimerState = new WorkState(Properties.Settings.Default.WorkTime);
            m_TimerState.NewStateGenerated += OnNewStateGenerated;

            m_Timer = new DispatcherTimer();
            m_Timer.Interval = TimeSpan.FromSeconds(1);
            m_Timer.Tick += OnEachSecond;
            m_Timer.Start();
        }

        public void Suspend()
        {
            m_Timer.IsEnabled = false;
            m_TimerState.Suspend();
        }

        public void Resume()
        {
            m_TimerState.Resume();
            m_Timer.IsEnabled = true;
        }

        private void OnEachSecond(object sender, EventArgs e)
        {
            m_TimerState.NextSecond();
        }

        private void OnNewStateGenerated(ITimerState newState)
        {
            m_TimerState.NewStateGenerated -= OnNewStateGenerated;
            m_TimerState.Dispose();

            newState.NewStateGenerated += OnNewStateGenerated;
            m_TimerState = newState;
        }

        public void Dispose()
        {
            m_Timer.Stop();
            m_Timer.Tick -= OnEachSecond;
            m_TimerState.NewStateGenerated -= OnNewStateGenerated;
            m_TimerState.Dispose();
        }
    }
}
