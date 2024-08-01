using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.EntityManager.ViewModels.Commands;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Modules.EntityManager.ViewModels
{
    [MessageOwner(MessageOwner.EntityManager)]
    public class EntityManagerViewModel : BaseModuleViewModel<D365EntityDescritpion>
    {
        private string token;
        private string fieldsMap;
        private int metadataType = 1;
        private string nameFilter;
        private string fieldFilter;
        private string filter;
        private string fieldUXFilter;
        //private readonly IEventAggregator eventAggregator;


        private CollectionViewSource FieldListViewSource = new CollectionViewSource();

        public EntityManagerViewModel(IModuleMessageRepository moduleMessageRepository,
            IUXUpdateMessageRepository  uXUpdateMessageRepository,
            ISearchHistoryService searchHistoryService,
            ITaskManager taskManager,
            IEventAggregator eventAggregator,
            IContainerProvider containerProvider
            ) :
            base(moduleMessageRepository, uXUpdateMessageRepository, searchHistoryService, taskManager, eventAggregator, containerProvider)
        {
            Load = new LoadCommand(this);
            FetchToken = new FetchTokenCommand(this);
            GetTableMap = new GetTableMapCommand(this);
            ClearFilter = new ClearFilterCommand(this);
            ClearFieldFilter = new ClearFieldFilterCommand(this);
            InitToken();

            //this.eventAggregator = eventAggregator;
            GetCustomFieldsCommand = new GetCustomFieldsCommand(this);
            eventAggregator.GetEvent<FieldMapEvent>().Subscribe(ShowFields);
            eventAggregator.GetEvent<TokenEvent>().Subscribe(ShowToken);
        }

        #region Propes
        public ICommand Load { get; set; }
        public ICommand FetchToken { get; set; }
        public ICommand GetCustomFieldsCommand { get; set; }
        public ICommand GetTableMap { get; set; }
        public ICommand ClearFilter { get; set; }

        public ICommand ClearFieldFilter { get; set; }

        public ICommand ColumnDetailCommand => new DelegateCommand(async (obj) =>
        {
            var description = obj as EntityFieldDescription;
            var entityService = ToolsContext.Current.UnityContainer.ResolveService<IEntityDefinitionService>();

            var result = await entityService.GetEntityChoiceAsync(description.EntityName, description.Name, Settings.Current.D365AccessToken);
            var seperator = ":";
            var FieldsMap = new StringBuilder("");
            foreach (var item in result.OptionSet.Options)
            {
                FieldsMap.Append($"{item.Label.LocalizedLabels[0].Label.Replace(" ", "").Trim()}{seperator}{item.Value},\r");

            }

            eventAggregator.GetEvent<FieldMapEvent>().Publish(FieldsMap.ToString());

        });

        //public ICommand ClearFieldFilter = new DelegateCommand((obj) =>
        //{
        //    var viewmodel = obj as EntityManagerViewModel;
        //    viewmodel.FieldNameFilter = string.Empty;
        //});

        /// <summary>
        /// the authorizion token to call dynamic api
        /// </summary>
        public string FieldUXFilter
        {
            get
            {
                return fieldUXFilter;
            }
            set
            {
                fieldUXFilter = value;
                RaisePropertyChanged("FieldUXFilter");
            }
        }

        /// <summary>
        /// the authorizion token to call dynamic api
        /// </summary>
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
                RaisePropertyChanged("Filter");
            }
        }

        /// <summary>
        /// the authorizion token to call dynamic api
        /// </summary>
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                RaisePropertyChanged("Token");
            }
        }

        public string FieldsMap
        {
            get
            {
                return fieldsMap;
            }
            set
            {
                fieldsMap = value;
                RaisePropertyChanged("FieldsMap");
            }
        }

        /// <summary>
        /// the type of medadate
        /// </summary>
        public int MetadataType
        {
            get
            {
                return MetadataType1;
            }
            set
            {
                MetadataType1 = value;
                RaisePropertyChanged("MetadataType");
            }
        }

        /// <summary>
        /// the string to filter the data
        /// </summary>
        public string NameFilter
        {
            get
            {
                return nameFilter;
            }
            set
            {
                nameFilter = value;
                RaisePropertyChanged("NameFilter");
                RefreshPageData();
            }
        }

        public string FieldNameFilter
        {
            get
            {
                return fieldFilter;
            }
            set
            {
                fieldFilter = value;
                RaisePropertyChanged("FieldNameFilter");
                RefresFieldshData();
            }
        }

        public ICollectionView FieldListView
        {
            get
            {
                return FieldListViewSource.View;
            }
            set
            {
                RaisePropertyChanged("FieldListView");
            }
        }

        public int MetadataType1 { get => metadataType; set => metadataType = value; }


        #endregion

        #region protected methods

        /// <summary>
        /// get the token from db
        /// </summary>
        protected void InitToken()
        {

            var token = Settings.Current.D365AccessToken;

            if (!string.IsNullOrEmpty(token))
            {
                Token = token;
            }
        }

        /// <summary>
        /// update UX items
        /// </summary>
        /// <param name="data"></param>
        protected override void UpdateItems(IExtractResult<BaseResourceMetadata> data)
        {
            if (data is ExtractResult<EntityDescription>)
            {
                var page = data as ExtractResult<EntityDescription>;
                foreach (var item in page.Collection)
                {
                    try
                    {

                        var vItem = new D365EntityDescritpion
                        {
                            Name = item.Name,
                            MessageOwner = MessageOwner.WebresourceManager,
                            DisplayName = item.DisplayName,
                            ObjectId = item.ObjectId
                        };

                        PageList.Add(vItem);
                    }
                    catch (Exception ex)
                    {
                        Error(ex.ToString());
                    }
                }

                RefreshPageData();
            }
        }

        /// <summary>
        /// filter the data
        /// </summary>
        /// <param name="description">the entity need to be filter</param>
        /// <returns></returns>
        protected override bool PageNameFilter(D365EntityDescritpion description)
        {
            return description.Name.Contains(NameFilter ?? "");
        }

        protected override void ItemSelectedChanged(BaseUXItemDescription data)
        {
            var item = data as D365EntityDescritpion;
            SetFieldDataSource(item.FieldItems);

            FieldsMap = item.FieldMap;
        }

        #endregion

        private void ShowFields(string fieldsMap)
        {
            //implement logic

            FieldsMap = fieldsMap;
        }

        private void ShowToken(string token)
        {
            Token = token;
        }


        private void RefresFieldshData()
        {
            if (FieldListViewSource != null)
            {
                FieldListViewSource.View?.Refresh();
            }
        }

        public void SetFieldDataSource(ObservableCollection<EntityFieldDescription> observableCollection)
        {
            FieldListViewSource.Source = observableCollection;
            FieldListViewSource.Filter += new FilterEventHandler(ViewSrcOnFileter);

            RaisePropertyChanged("FieldListView");
        }

        private void ViewSrcOnFileter(object sender, FilterEventArgs e)
        {
            var data = e.Item as EntityFieldDescription;
            e.Accepted = data.Name.ToLower().Contains(FieldNameFilter?.ToLower() ?? "");
        }
    }
}
