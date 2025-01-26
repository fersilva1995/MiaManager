using MiaManager.EventsArgs;
using MiaManager.Models;
using MiaManager.Services;

namespace MiaManager.ViewModels
{
    public class UserItemViewModel : BaseViewModel
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
            set { id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public UserItemViewModel()
        {

        }

        public UserItemViewModel(User user)
        {
            Name = user.Name;
            Id = user.Id;
        }

    }
}
