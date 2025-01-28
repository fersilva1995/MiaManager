using MiaGRPC;
using MiaManager.Commands;
using MiaManager.EventsArgs;
using MiaManager.Services;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class UserViewModel : BaseViewModel
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

        /*private DataViewModel selected = new();
        public DataViewModel Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
                if (value != null)
                    DataService.Instance.Selected = new()
                    {
                        Id = value.Id,
                        Name = value.Name,
                    };
            }
        }*/


        public ObservableCollection<DataViewModel> Data { get; set; } = [];


        public AddUserCommand AddUserCommand { get; set; }
        public AddCommand AddCommand { get; set; }
        public LoadCommand LoadCommand { get; set; }
        public RemoveAllCommand RemoveAllCommand { get; set; }
        public LoadAllDataCommand LoadAllDataCommand { get; set; }

        public UserViewModel()
        {
            UserService.Instance.SelectUserEvent += SelectUser;
            AddUserCommand = new(this);
            AddCommand = new(this);
            LoadCommand = new(this);
            RemoveAllCommand = new(this);
            LoadAllDataCommand = new(this);

        }

        public void ClearDataType(string type)
        {
            Data.Clear();
           /* for (int index = 0; index < Data.Count; index++)
            {
                DataViewModel data = Data[index];
                if (data.Type == type)
                {
                    Data.Remove(data);
                    index--;
                
            }*/

        }

        public async void LoadData(string type)
        {
            List<ImageData> data = await DataService.Instance.GetList(type);
            ClearDataType(type);

            foreach (ImageData imageData in data)
                Data.Add(new()
                {
                    Id = imageData.Id,
                    Name = imageData.Name,
                    Type = type,
                });
        }

        private void SelectUser(object? sender, EventArgs e)
        {
            SelectEventArg arg = (SelectEventArg)e;
            Name = arg.Name;
            Id = arg.Id.ToString();
        }


    }
}
