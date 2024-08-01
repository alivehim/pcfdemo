using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Core.Extensions;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class PublishCommand : ICommand
    {
        private readonly FileUXItemDescription fileUXItemDescription;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public PublishCommand(FileUXItemDescription fileUXItemDescription)
        {
            this.fileUXItemDescription = fileUXItemDescription;
        }

        public bool CanExecute(object parameter)
        {
            return !fileUXItemDescription.IsWaiting;
        }


        /// <summary>
        /// upload webresource file to server
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            fileUXItemDescription.TaskStage = Core.Models.TaskStage.Doing;
            fileUXItemDescription.IsWaiting = true;
            var webresourceservice = ToolsContext.Current.UnityContainer.ResolveService<IWebresourceServers>();

            Task.Run(async () =>
            {
                await webresourceservice.PublishAsync(fileUXItemDescription, fileUXItemDescription.ObjectId, parameter.ToString());

            }).ContinueWith(token =>
            {
                fileUXItemDescription.IsWaiting = false;
                if (token.IsFaulted) throw token.Exception;

                fileUXItemDescription.TaskStage = Core.Models.TaskStage.Done;

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}
