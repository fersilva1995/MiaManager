using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Models
{
    public class Report
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public double Threshold { get; set; } = 0;
        public List<Target> Targets { get; set; } = [];
        public Svm? Svm { get; set; } = null;
    }
}
