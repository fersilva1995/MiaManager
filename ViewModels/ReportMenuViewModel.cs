using MiaManager.Models;
using MiaManager.Services;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class ReportMenuViewModel : BaseViewModel
    {
        public ObservableCollection<ReportViewModel> Items { get; set; } = [];

        private ReportViewModel selectedReport = new();
        public ReportViewModel SelectedReport
        {
            get
            {
                return selectedReport;
            }
            set
            {
                selectedReport = value;
                OnPropertyChanged(nameof(SelectedReport));
            }
        }

        public void Load()
        {
            Items.Clear();
            foreach (Report report in ReportService.Instance.Reports)
                Items.Add(new()
                {
                    Id = report.Id,
                    Name = report.Name,
                });
        }

        public void Add()
        {

        }




    }
}
