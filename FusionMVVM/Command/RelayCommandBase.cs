using System;
using System.Windows.Input;

namespace FusionMVVM.Command
{
    public abstract class RelayCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Func<bool> _canExecuteAction;

        /// <summary>
        /// Initializes a new instance of the RelayCommandBase class.
        /// </summary>
        /// <param name="canExecuteAction"></param>
        protected RelayCommandBase(Func<bool> canExecuteAction)
        {
            _canExecuteAction = canExecuteAction;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null || _canExecuteAction.Invoke();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Raises the CanExecuteChanged event on the command.
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
