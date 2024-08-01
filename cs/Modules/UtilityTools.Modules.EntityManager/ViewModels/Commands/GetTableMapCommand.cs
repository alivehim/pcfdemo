using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.Modules.EntityManager.ViewModels.Commands
{
    public class GetTableMapCommand:ICommand
    {
        private readonly EntityManagerViewModel  entityManagerViewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public GetTableMapCommand(EntityManagerViewModel entityManagerModule)
        {
            this.entityManagerViewModel = entityManagerModule;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();
            var FieldsMap = new StringBuilder("");
            foreach (var item in entityManagerViewModel.PageList)
            {
                FieldsMap.Append($"{item.Name.Replace("aia_eaa_","")}:\"{item.Name}\",\r");
            }
            eventAggregator.GetEvent<FieldMapEvent>().Publish(FieldsMap.ToString());
        }
    }
}
