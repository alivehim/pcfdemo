using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class CanvasOnenoteSectionDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public event Action<IDialogResult> RequestClose;

        public ObservableCollection<OnenoteSectionDialogItemViewModel> OnenoteBookItems { get; set; } = new ObservableCollection<OnenoteSectionDialogItemViewModel>();

        public OnenoteSectionDialogItemViewModel selectedItem { get; set; }
        public OnenoteSectionDialogItemViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }



        //private readonly IOnenoteServiceFactory onenoteServiceFactory;
        private readonly IGraphOnenoteService graphOnenoteService;

        private string _title = "Onenote";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool isWaiting;
        public bool IsWaiting
        {
            get { return isWaiting; }
            set { SetProperty(ref isWaiting, value); }
        }

        public CanvasOnenoteSectionDialogViewModel(IGraphOnenoteService graphOnenoteService)
        {
            this.graphOnenoteService = graphOnenoteService;
        }

        public ICommand OpenOnenoteSectionCommand => new Prism.Commands.DelegateCommand(() =>
        {

            IsWaiting = true;

            Task.Run(async () =>
            {
                //var service = onenoteServiceFactory.GetHandler(OnenoteSource.MicrosoftGraph);
                var defaultBook = "https://graph.microsoft.com/v1.0/users/8c7f11f5-2cec-4b01-a363-155fe5b8f457/onenote/notebooks/1-9d8104a7-e0c8-4139-ba9a-1c1820fa82f9/sections";
                return await graphOnenoteService.GetOnenoteSectionsAsync(defaultBook);


            }).ContinueWith((res) =>
            {
                IsWaiting = false;
                if (res.Exception != null)
                {
                    return;
                }

                OnenoteBookItems.Clear();

                OnenoteBookItems.AddRange(res.Result.Select(p => new OnenoteSectionDialogItemViewModel
                {

                    Name = p.displayName,
                    PagesUrl = p.pagesUrl,
                    SectionUrl = p.self
                }));

            }, TaskScheduler.FromCurrentSynchronizationContext());

        });

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;


            RaiseRequestClose(new DialogResult(result));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            if (SelectedItem != null)
            {
                var parameters = new DialogParameters();
                parameters.Add("pagesUrl", SelectedItem.PagesUrl);
                parameters.Add("sectionName", SelectedItem.Name);
                RequestClose?.Invoke(new DialogResult(dialogResult.Result, parameters));

            }
            else
            {
                RequestClose?.Invoke(dialogResult);

            }

        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }

    }
}
