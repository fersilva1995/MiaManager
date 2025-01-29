using MiaManager.Commands.ReportCommand;
using MiaManager.EventsArgs;
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

        private double threshold = 0;
        public double Threshold
        {
            get { return threshold; }
            set
            {
                threshold = value;
                OnPropertyChanged(nameof(Threshold));
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


        public ObservableCollection<SvmItemViewModel> Svms { get; set; } = [];
        public ObservableCollection<TargetViewModel> Targets { get; set; } = [];
        public ObservableCollection<ReportResultViewModel> Reports { get; set; } = [];



        public AddReportCommand AddReportCommand { get; set; }
        public ExecuteReportCommand ExecuteReportCommand { get; set; }
        public UpdateReportCommand UpdateReportCommand { get; set; }

        public ReportViewModel()
        {
            LoadSvm();
            LoadTargets();

            AddReportCommand = new(this);
            ExecuteReportCommand = new(this);
            UpdateReportCommand = new(this);
            ReportService.Instance.SelectEvent += Select;
        }

        private void Select(object? sender, EventArgs e)
        {
            SelectEventArg arg = (SelectEventArg)e;
            Report? report = ReportService.Instance.Reports.Where(d => d.Id == Convert.ToInt64(arg.Id)).FirstOrDefault();
            if (report != null)
            {
                Id = report.Id;
                Name = report.Name;
                Threshold = report.Threshold;
                if (report.Svm != null)
                    SelectedSvm = Svms.Where(s => s.Id == report.Svm.Id).First();

                foreach (Target target in report.Targets)
                {

                    TargetViewModel? t = Targets.Where(t => t.Id == target.Id).FirstOrDefault();
                    if(t != null)
                    {
                        t.InputFiles.Clear();
                        t.OutputFiles.Clear();

                        t.CurrentFeatureNumber = target.CurrentFeatureNumber;
                        t.FileCount = target.InputFiles.Count;
                        foreach (var file in target.InputFiles)
                            t.InputFiles.Add(file);

                        foreach (var file in target.OutputFiles)
                            t.OutputFiles.Add(file);
                    }
                }

                foreach(TargetViewModel t in Targets)
                {
                    if(!report.Targets.Select(t => t.Id).Contains(t.Id))
                    {
                        t.CurrentFeatureNumber = 0;
                        t.FileCount = 0;
                        t.InputFiles.Clear();
                        t.OutputFiles.Clear();
                    }
                }

                Reports.Clear();
                List<ReportResult> results = ReportResultService.Instance.Reports.Where(r => r.ReportId == report.Id).ToList();

                foreach (ReportResult result in results)
                {
                    result.Load();
                    Reports.Add(new()
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Precision = Math.Round(result.Precision * 100, 2),
                        Recall = Math.Round(result.Recall * 100, 2),
                        F1 = Math.Round(result.F1Score * 100, 2),
                        Threshold = result.Threshold,
                        
                    });
                }


            }
        }
        public void LoadSvm()
        {
            foreach (Svm svm in SvmService.Instance.Elements)
                Svms.Add(new()
                {
                    Id = svm.Id,
                    Name = svm.Name,
                });
        }

        public void LoadTargets()
        {
            foreach (User user in UserService.Instance.Users)
                Targets.Add(new()
                {
                    Id = user.Id,
                    Name = user.Name,
                    CurrentFeatureNumber = user.Features.Count(),
                });
        }


    }
}
