using MiaManager.Commands;
using MiaManager.Models;
using MiaManager.Services;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class UsersMenuViewModel : BaseViewModel
    {
        public ObservableCollection<UserItemViewModel> Users { get; set; } = [];

        private string softwareName = "MIA";
        public string SoftwareName
        {
            get { return softwareName; }
            set
            {
                softwareName = value;
                OnPropertyChanged(nameof(SoftwareName));

            }
        }


        private UserItemViewModel selectedUser = new();
        public UserItemViewModel SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                if (value != null)
                    UserService.Instance.SetSelectedUser(value.Id);
                else
                    UserService.Instance.SelectedUser = null;
            }
        }

        public RefreshUsersCommand RefreshUsersCommand { get; set; }
        public RemoveUserCommand RemoveUserCommand { get; set; }


        public UsersMenuViewModel()
        {
            RefreshUsersCommand = new(this);
            RemoveUserCommand = new(this);
            LoadUsers();
        }

        public void LoadUsers()
        {
            while (!UserService.Instance.Loaded) { }
            Users.Clear();
            foreach (User user in UserService.Instance.Users)
                Users.Add(new(user));
        }

        public void Remove()
        {
            UserService.Instance.Remove();
            UserItemViewModel? userItemViewModel = Users.Where(u => u.Id == selectedUser.Id).FirstOrDefault();
            if (userItemViewModel != null)
                Users.Remove(userItemViewModel);

        }
    }
}
