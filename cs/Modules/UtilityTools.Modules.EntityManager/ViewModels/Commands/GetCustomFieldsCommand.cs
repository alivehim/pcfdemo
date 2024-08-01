using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Modules.EntityManager.Models;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Modules.EntityManager.ViewModels.Commands
{
    public class GetCustomFieldsCommand : ICommand
    {
        private readonly EntityManagerViewModel entityManagerViewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public GetCustomFieldsCommand(EntityManagerViewModel entityManagerViewModel)
        {
            this.entityManagerViewModel = entityManagerViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return !entityManagerViewModel.IsWaiting;
        }


        /// <summary>
        /// upload webresource file to server
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            var token = (string)values[0];
            var d365EntityDescritpion = (D365EntityDescritpion)values[2];
            var metadataType = (int)values[1];

            d365EntityDescritpion.TaskStage = TaskStage.Doing;

            d365EntityDescritpion.IsWaiting = true;
            var entityService = ToolsContext.Current.UnityContainer.ResolveService<IEntityDefinitionService>();
            var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();

            Task.Run(async () =>
            {
                var FieldsMap = new StringBuilder("");
                if (metadataType == (int)MetadataType.Entity)
                {
                    var definition = await entityService.GetEntityAsync(d365EntityDescritpion.Name, token);


                    var fields = definition.Attributes.Where(p => p.IsCustomAttribute && p.DisplayName != null && p.DisplayName.LocalizedLabels.Count > 0);

                    StringBuilder sb = new StringBuilder();

                    foreach (var item in fields)
                    {
                        if (item.DisplayName != null && item.DisplayName.LocalizedLabels.Count > 0 &&
                            !item.LogicalName.Contains("_sys_")
                        )
                        {
                            sb.Append($"\"{item.LogicalName}\":\"{item.DisplayName.LocalizedLabels[0].Label}\",\n");

                        }
                    }

                    File.WriteAllText("1.txt", sb.ToString());

                    return fields.Select(p => new EntityResult
                    {
                        Name = p.LogicalName,
                        ColumnType = p.AttributeType,
                        EntityName = d365EntityDescritpion.Name,
                        DisplayName= p.DisplayName.LocalizedLabels[0].Label
                    }).ToList();

                }
                else
                {
                    var definition = await entityService.GetChoiceAsync(d365EntityDescritpion.ObjectId, token);
                    var seperator = ":";
                    foreach (var item in definition.Options)
                    {
                        FieldsMap.Append($"{item.Label.LocalizedLabels[0].Label.Replace(" ", "").Trim()}{seperator}{item.Value},\r");
                    }
                    eventAggregator.GetEvent<FieldMapEvent>().Publish(FieldsMap.ToString());

                    return definition.Options.Select(p => new EntityResult
                    {
                        Name = p.Label.LocalizedLabels[0].Label
                    }).ToList();
                }

            }).ContinueWith((result) =>
            {
                if (result.Result != null)
                {
                    d365EntityDescritpion.AddFields(result.Result);
                }
                d365EntityDescritpion.TaskStage = TaskStage.None;
                d365EntityDescritpion.IsWaiting = false;

                entityManagerViewModel.SetFieldDataSource(d365EntityDescritpion.FieldItems);

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}
