using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Services
{
    public class ProgressService
    {
        #region SINGLETON

        private ProgressService()
        {

        }

        private static ProgressService? instance = null;
        public static ProgressService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        #endregion


        public double Max { get; set; } = 100;
        public double Value { get; set; } = 0;
        public double Steps { get; set; } = 100;
        public string CurrentText { get; set; } = string.Empty;

        private bool stop = false;
        public bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                Value = stop ? Max : 0;
                StepEvent?.Invoke(this, new EventArgs());

            }
        }

        public bool Finished
        {
            get
            {
                return Value >= Max;
            }
        }


        public EventHandler? StepEvent { get; set; }

        public void Step(string text = "")
        {
            double step = Steps / Max;
            if (text != "")
                CurrentText = text;
            Value += step;
            StepEvent?.Invoke(this, new EventArgs());
        }
    }
}
