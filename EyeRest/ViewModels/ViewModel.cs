using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace EyeRest.ViewModels
{
    /// <summary>
    /// Represents abstract view model.
    /// </summary>
    internal abstract class ViewModel<TControl> : INotifyPropertyChanged
        where TControl : Control
    {
        protected readonly TControl _control;

        /// <summary>
        /// Occurs on change of some property.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="ViewModel{TControl}"/> class.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <exception cref="ArgumentNullException">Control is null.</exception>
        protected ViewModel(TControl control)
        {
            if( control == null )
                throw new ArgumentNullException("control");

            _control = control;
        }

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
    }
}
