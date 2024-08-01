using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Modules.AppSettings.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string name;
        private string password;


        private readonly IUserService _userService;

        public UserViewModel(IUserService userService)
        {
            _userService = userService;

            Add = new DelegateCommand((obj) =>
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
                {
                    _userService.Add(new Data.Domain.ResourceUser
                    {
                        Name = name,
                        Password = password
                    });
                    LoadData();
                }
            });
            Delete = new DelegateCommand((obj) =>
            {
                var item = obj as UserItemViewModel;
                _userService.DeleteByName(item.Name);
                LoadData();
            });

            LoadData();
        }

        public ICommand Add { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Save { get; set; }


        public ObservableCollection<UserItemViewModel> Items { get; set; } = new ObservableCollection<UserItemViewModel>();

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }

        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }

        }



        private void LoadData()
        {
            Items.Clear();

            var items = _userService.GetAll();

            foreach (var item in items)
            {
                Items.Add(new UserItemViewModel { Name = item.Name, Password = item.Password }); ;
            }
        }

    }
}
