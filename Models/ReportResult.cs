using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Models
{
    public class ReportResult
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public List<User> Users { get; set; } = [];
        public List<Svm> Svms { get; set; } = [];

        public int TruePositive { get; set; } = 0;
        public int FalseNegative { get; set; } = 0;
        public int FalsePositive {  get; set; } = 0;
        public int TrueNegative { get; set; } = 0;

        public Dictionary<string, string> InputParameters { get; set; } = [];
        public Dictionary<string, List<string>> OutputParameters { get; set; } = [];

    }
}
