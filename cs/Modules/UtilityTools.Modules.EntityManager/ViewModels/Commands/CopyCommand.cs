using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UtilityTools.Modules.EntityManager.ViewModels.Commands
{
    public class CopyCommand:ICommand
    {
        private readonly D365EntityDescritpion d365EntityDescritpion;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CopyCommand(D365EntityDescritpion d365EntityDescritpion)
        {
            this.d365EntityDescritpion = d365EntityDescritpion;
        }

        public bool CanExecute(object parameter)
        {
            return !d365EntityDescritpion.IsWaiting;
        }


        /// <summary>
        /// upload webresource file to server
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Clipboard.SetDataObject(d365EntityDescritpion.Name);
        }
    }
}
