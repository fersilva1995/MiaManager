using MiaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.ViewModels
{
    public class SvmItemViewModel : BaseViewModel
    {
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

 


        public SvmItemViewModel()
        {

        }

        public SvmItemViewModel(User user)
        {
            Name = user.Name;
            Id = user.Id;
        }

    }
}
