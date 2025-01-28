using MiaManager.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.ViewModels
{
    public class ReportResultViewModel : BaseViewModel
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

        private double precision;

        public double Precision
        {
            get { return precision; }
            set
            {
                precision = value;
                OnPropertyChanged(nameof(Precision));
            }
        }

        private double recall;

        public double Recall
        {
            get { return recall; }
            set
            {
                recall = value;
                OnPropertyChanged(nameof(Recall));
            }
        }

        private double f1;

        public double F1
        {
            get { return f1; }
            set
            {
                f1 = value;
                OnPropertyChanged(nameof(F1));

            }
        }

        public RemoveReportResult RemoveReportResult { get; set; }
        public ShowReportResult ShowReportResult { get; set; }

        public ReportResultViewModel()
        {
            RemoveReportResult = new(this);
            ShowReportResult = new(this);
        }




    }
}
