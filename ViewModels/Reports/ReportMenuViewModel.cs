using MiaManager.Commands.MiaCommands;
using MiaManager.Models;
using MiaManager.Services;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class ReportMenuViewModel : BaseViewModel
    {
        private string header = string.Empty;
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged(nameof(Header));
            }
        }


        public ObservableCollection<ReportItemViewModel> Reports { get; set; } = [];

        public RefreshReportsCommand RefreshReportsCommand { get; set; }

        public ReportMenuViewModel()
        {
            RefreshReportsCommand = new(this);
            Load();
        }

        private ReportItemViewModel selected = new();
        public ReportItemViewModel Selected
        {
            get
            {
                return selected;
            }
            set
            {
     
                if(value != null)
                {
                    selected = value;
                    OnPropertyChanged(nameof(Selected));
                    ReportService.Instance.SetSelected(value.Id);
                }

            }
        }

        public void Load()
        {
            Reports.Clear();
            foreach (Report report in ReportService.Instance.Reports)
                Reports.Add(new()
                {
                    Id = report.Id,
                    Name = report.Name,
                });
        }
    }
}
