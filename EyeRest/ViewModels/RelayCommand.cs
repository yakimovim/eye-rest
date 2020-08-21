using System;
using System.Windows.Input;

namespace EyeRest.ViewModels
{
    /// <summary>
    /// Represents command from delegate.
    /// </summary>
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

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

            _execute = execute;
            _canExecute = canExecute;
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

            return _canExecute != null ? _canExecute(parameter) : true;
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
            _execute(parameter);
        }
    }
}
