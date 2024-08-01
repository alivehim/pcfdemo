using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UtilityTools.Modules.EntityManager.ViewModels.Commands
{
    public class CopyFieldCommand : ICommand
    {
        private readonly EntityFieldDescription  entityFieldDescription;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CopyFieldCommand(EntityFieldDescription entityFieldDescription)
        {
            this.entityFieldDescription = entityFieldDescription;
        }

        public bool CanExecute(object parameter)
        {
            return !entityFieldDescription.IsWaiting;
        }


        /// <summary>
        /// upload webresource file to server
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Clipboard.SetDataObject(entityFieldDescription.Name);
        }
    }
}
