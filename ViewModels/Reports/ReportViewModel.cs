using MiaManager.Models;
using MiaManager.Services;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private long id;
        public long Id
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

        private SvmItemViewModel selectedSvm = new();
        public SvmItemViewModel SelectedSvm
        {
            get { return selectedSvm; }
            set
            {
                selectedSvm = value;
                OnPropertyChanged(nameof(SelectedSvm));
            }
        }
        public ObservableCollection<SvmItemViewModel> SvmItemViewModels { get; set; } = [];

        public ObservableCollection<TargetViewModel> TargetItemViewModels { get; set; }



        public ReportViewModel()
        {
            LoadSvm();
            LoadTargets();
        }

        public void LoadSvm()
        {
            foreach (Svm svm in SvmService.Instance.Elements)
                SvmItemViewModels.Add(new()
                {
                    Id = svm.Id,
                    Name = svm.Name,
                });
        }

        public void LoadTargets()
        {
            foreach (User user in UserService.Instance.Users)
                TargetItemViewModels.Add(new()
                {
                    Id = user.Id,
                    Name = user.Name,
                    CurrentFeatureCount = user.Features.Count(),
                });
        }
    }
}
