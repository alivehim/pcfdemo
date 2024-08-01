using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UtilityTools.Commands
{
    public sealed class DummyCommand : ICommand
    {

        #region Implementation of ICommand
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("dummy");
        }

        #endregion
    }
}
