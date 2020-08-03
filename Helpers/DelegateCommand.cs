using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaterialMesignWPFApp.Helpers
{
    public class DelegateCommand: ICommand
    {
        #region Field
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region Constructors
        public DelegateCommand(Action<object> execute):this(execute, null) { }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region Icommand members
        public bool CanExecute(object parametr)
        {
            return _canExecute?.Invoke(parametr) ?? true; 
        }
        #endregion
    }
}
