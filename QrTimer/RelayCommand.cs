using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace br.corp.bonus630.plugin.QrTimer
{
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {

        }
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;

        }
        public bool CanExecute(object parameter)
        {
            if(_canExecute!=null)
                _canExecute((T)parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if(_execute!=null)
                _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if(CanExecuteChanged!=null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

    }
}
