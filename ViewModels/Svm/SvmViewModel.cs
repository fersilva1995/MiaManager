using MiaManager.Commands;
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

        public SvmViewModel()
        {
            AddSvmCommand = new(this);
        }
    }
}
