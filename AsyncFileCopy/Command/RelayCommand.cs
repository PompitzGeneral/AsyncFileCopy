using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncFileCopy.Command
{
    public class RelayCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public RelayCommand( Predicate<object> canExecuteDelegate, Action<object> executeDelegate )
        {
            CanExecuteDelegate = canExecuteDelegate;
            ExecuteDelegate = executeDelegate;
        }

        public RelayCommand( Action<object> executeDelegate )
        {
            ExecuteDelegate = executeDelegate;
        }

        public RelayCommand()
        {
            // ExecuteDelegate = executeDelegate;
        }

        public bool CanExecute( object parameter )
        {
            if ( CanExecuteDelegate != null )
                return CanExecuteDelegate( parameter );

            return true;// if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute( object parameter )
        {
            if ( ExecuteDelegate != null )
                ExecuteDelegate( parameter );
        }
    }
}
