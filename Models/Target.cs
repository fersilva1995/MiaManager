using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Models
{
    public class Target
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CurrentFeatureNumber { get; set; } = 0;
        public List<string> InputFiles { get; set; } = [];
        public List<string> OutputFiles { get; set; } = [];
    }
}
