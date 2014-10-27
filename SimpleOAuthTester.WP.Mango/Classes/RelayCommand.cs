using System;
using System.Windows.Input;

namespace SimpleOAuthTester.WP.Mango.Classes
{
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> _executeAction;
        private readonly Func<T, bool> _canExecuteFunctionWithParam;
        private readonly Func<bool> _canExecuteFunctionWithoutParam;

        public RelayCommand(Action<T> executeAction)
            : this(executeAction, (Func<bool>)null)
        {
        }

        public RelayCommand(Action<T> executeAction, Func<T, bool> canExecuteFunction)
        {
            _executeAction = executeAction;
            _canExecuteFunctionWithParam = canExecuteFunction;
        }

        public RelayCommand(Action<T> executeAction, Func<bool> canExecuteFunction)
        {
            _executeAction = executeAction;
            _canExecuteFunctionWithoutParam = canExecuteFunction;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteFunctionWithoutParam != null)
            {
                return _canExecuteFunctionWithoutParam();
            }
            else if (_canExecuteFunctionWithParam != null)
            {
                return _canExecuteFunctionWithParam((T)parameter);
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            _executeAction((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler temp = CanExecuteChanged;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }
    }

    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _executeAction;
        private readonly Func<bool> _canExecuteFunction;

        public RelayCommand(Action executeAction)
            : this(executeAction, null)
        {
        }

        public RelayCommand(Action executeAction, Func<bool> canExecuteFunction)
        {
            _executeAction = executeAction;
            _canExecuteFunction = canExecuteFunction;
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteFunction != null)
            {
                return _canExecuteFunction();
            }
            else
            {
                return true;
            }
        }

        public void Execute()
        {
            Execute(null);
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler temp = CanExecuteChanged;
            if (temp != null)
            {
                UIHelper.SafeDispatch(() => temp(this, new EventArgs()));
            }
        }
    }
}
