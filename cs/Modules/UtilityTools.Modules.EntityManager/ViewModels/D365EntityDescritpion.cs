using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.EntityManager.Models;
using UtilityTools.Modules.EntityManager.ViewModels.Commands;

namespace UtilityTools.Modules.EntityManager.ViewModels
{
    public class D365EntityDescritpion : BaseUXItemDescription
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ObjectId { get; set; }

        public string FieldMap { get; set; }

        public ICommand CopyCommand { get; set; }
        public ICommand GetFieldMapCommand { get; set; }

        private ObservableCollection<EntityFieldDescription> fieldItems = new ObservableCollection<EntityFieldDescription>();

        public ObservableCollection<EntityFieldDescription> FieldItems
        {
            get { return fieldItems; }
            set { SetProperty(ref fieldItems, value); }
        }

        public D365EntityDescritpion()
        {
            CopyCommand = new CopyCommand(this);

            GetFieldMapCommand = new DelegateCommand((obj) =>
            {
                var item = obj as D365EntityDescritpion;
                var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();
                eventAggregator.GetEvent<FieldMapEvent>().Publish(item.FieldMap);
            });
        }

        private void ViewSrcOnFileter(object sender, FilterEventArgs e)
        {
            var data = e.Item as EntityFieldDescription;
            e.Accepted = true;
        }

        public void AddFields(IList<EntityResult> list)
        {
            fieldItems.Clear();
            fieldItems.AddRange(list.Select(p => new EntityFieldDescription { EntityName =p.EntityName,  Name = p.Name, ColumnType = p.ColumnType, DisplayName=p.DisplayName }));

            FieldMap = GetFiledMapString();
        }

        private string GetFiledMapString()
        {
            var fieldsMap = new StringBuilder("");

            foreach (var item in fieldItems)
            {
                fieldsMap.Append($"{item.Name}:\"{item.Name}\",\r");
            }

            return fieldsMap.ToString();

        }


    }
}
