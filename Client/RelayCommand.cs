using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Implementation of the <see cref="System.Windows.Input.ICommand"/>.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> execute;
        private Predicate<T> canExecute;
        private Dispatcher dispatcher;

        /// <summary>
        /// Initializes the new instance of the <see cref="RelayCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Delegete to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
        }

        public bool CanExecute(object parameter)
        {
            return (canExecute == null) ? true : canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        /// <summary>
        /// Raises <see cref="System.Windows.Input.ICommand.CanExecuteChanged"/> on the dispatcher thread if needed, otherwise on the current thread.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;

            if (handler != null)
            {
                if (dispatcher != null && !dispatcher.CheckAccess())
                    dispatcher.Invoke(new Action(RaiseEvent));
                else
                    handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises <see cref="System.Windows.Input.ICommand.CanExecuteChanged"/> on the dispatcher thread asynchronously if needed.
        /// </summary>
        public void RaiseCanExecuteChangedAsync()
        {
            var handler = CanExecuteChanged;

            if (handler != null)
            {
                if (dispatcher != null && !dispatcher.CheckAccess())
                    dispatcher.BeginInvoke(new Action(RaiseEvent));
                else
                    handler(this, EventArgs.Empty);
            }
        }

        private void RaiseEvent()
        {
            var handler = CanExecuteChanged;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            : base(execute, canExecute)
        {
        }
    }
}
