using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Models
{
    public class Data
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<byte> Value { get; set; } = [];
        public string Reference { get; set; } = string.Empty ;
    }
}
