using MiaManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.ViewModels
{
    public class ProgressViewModel : BaseViewModel
    {
        private double max = 100;
        public double Max
        {
            get { return max; }
            set
            {
                max = value;
                OnPropertyChanged(nameof(Max));

            }
        }

        private double val = 0;
        public double Value
        {
            get { return val; }
            set
            {
                val = value; OnPropertyChanged(nameof(Value));
            }
        }

        private string text = "progresso";
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }


        public ProgressViewModel()
        {
            Max = ProgressService.Instance.Max;
            ProgressService.Instance.StepEvent += Step;
        }

        private void Step(object? sender, EventArgs e)
        {
            Value = ProgressService.Instance.Value;
            Text = ProgressService.Instance.CurrentText;
        }
    }
}
