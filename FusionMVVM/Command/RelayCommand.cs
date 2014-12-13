using System;

namespace FusionMVVM.Command
{
    public class RelayCommand : RelayCommandBase
    {
        private readonly Action _action;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
            : this(action, () => true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecuteAction"></param>
        public RelayCommand(Action action, Func<bool> canExecuteAction)
            : base(canExecuteAction)
        {
            _action = action;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (parameter == null) _action();
        }
    }
}
