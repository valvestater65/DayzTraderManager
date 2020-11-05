using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DayzTraderManager.Commands
{
    public class ParameterLessCommand : ICommand
    {
        private Action action;
        private bool _canExecute;


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ParameterLessCommand(Action action, bool canExecute)
        {
            this.action = action;
            _canExecute = canExecute;
        }

        public ParameterLessCommand(Action action): this(action,true)
        {

        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
