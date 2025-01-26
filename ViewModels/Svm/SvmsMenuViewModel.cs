using MiaManager.Commands;
using MiaManager.Models;
using MiaManager.Services;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class SvmsMenuViewModel : BaseViewModel
    {
        public ObservableCollection<SvmItemViewModel> Svms { get; set; } = [];

        private string header = "MIA";
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged(nameof(Header));

            }
        }


        private SvmItemViewModel selected = new();
        public SvmItemViewModel Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
                if (value != null)
                    SvmService.Instance.SetSelected(value.Id);
                else
                    SvmService.Instance.Selected = null;
            }
        }

        public RefreshSvmsCommand RefreshSvmsCommand { get; set; }


        public SvmsMenuViewModel()
        {
            RefreshSvmsCommand = new(this);
            Load();
        }

        public void Load()
        {
            /*
            while (!UserService.Instance.Loaded) { }
            Svms.Clear();
            foreach (User user in UserService.Instance.Users)
                Svms.Add(new(user));*/
        }

    }
}
