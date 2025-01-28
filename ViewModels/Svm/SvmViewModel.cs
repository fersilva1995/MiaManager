using MiaGRPC;
using MiaManager.Commands;
using MiaManager.Commands.SvmCommands;
using MiaManager.EventsArgs;
using MiaManager.Models;
using MiaManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace MiaManager.ViewModels
{
    public class SvmViewModel : BaseViewModel
    {
        private string id = string.Empty;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));

            }
        }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private bool createNegative = false;
        public bool CreateNegative
        {
            get { return createNegative; }
            set
            {
                createNegative = value;
                OnPropertyChanged(nameof(CreateNegative));
            }
        }

        private bool createUnknown;
        public bool CreateUnknown
        {
            get { return createUnknown; }
            set
            {
                createUnknown = value;
                OnPropertyChanged(nameof(CreateUnknown));
            }
        }

        public ObservableCollection<UserItemViewModel> Users { get; set; } = [];

        public bool? IsAllSelected
        {
            get
            {
                var selected = Users.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value, Users);
                    OnPropertyChanged(nameof(IsAllSelected));
                }
            }
        }

        private static void SelectAll(bool select, IEnumerable<UserItemViewModel> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }


        public AddSvmCommand AddSvmCommand { get; set; }
        public UpdateSvmCommand UpdateSvmCommand { get; set; }  


        public SvmViewModel()
        {
            AddSvmCommand = new(this);
            UpdateSvmCommand = new(this);
            SvmService.Instance.SelectEvent += Select;
        }

        private void Select(object? sender, EventArgs e)
        {
            Users.Clear();  
            SelectEventArg arg = (SelectEventArg)e;
            Svm? svm = SvmService.Instance.Elements.Where(d => d.Id ==  arg.Id.ToString()).FirstOrDefault();    
            if (svm != null)
            {
                Id = svm.Id;
                Name = svm.Name;
                CreateNegative = svm.CreateNegative;
                CreateUnknown = svm.CreateUnknown;

                foreach (User user in UserService.Instance.Users) 
                    Users.Add(new()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        IsSelected = svm.Users.Contains(user.Id),
                    });
                   
            }

           
        }
    }
}
