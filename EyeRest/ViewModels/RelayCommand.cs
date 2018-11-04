using System;
using System.Windows.Input;
using EyeRest.Properties;

namespace EyeRest.ViewModels
{
    /// <summary>
    /// Represents command from delegate.
    /// </summary>
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> m_Execute;
        private readonly Func<object, bool> m_CanExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Function to get if command can be executed.</param>
        /// <exception cref="ArgumentNullException">Either argument is null.</exception>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            m_Execute = execute;
            m_CanExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <exception cref="ArgumentNullException">Either argument is null.</exception>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        { }

        /// <summary>
        /// Returns if command can be executed.
        /// </summary>
        /// <param name="parameter">Command's parameter.</param>
        /// <returns>True, if command can be executed. False, otherwise.</returns>
        public bool CanExecute(object parameter)
        {

            return m_CanExecute != null ? m_CanExecute(parameter) : true;
        }

        /// <summary>
        /// Occurs when value of <see cref="CanExecute"/> method is changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="parameter">Command's parameter.</param>
        public void Execute(object parameter)
        {
            m_Execute(parameter);
        }
    }
}
