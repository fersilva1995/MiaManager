using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;    

        public List<Data> Images { get; set; } = [];
        public List<Data> AugmentedImages { get; set; } = [];
        public List<Data> Faces { get; set; } = [];
        public List<Data> Features { get; set; } = [];  

    }
}
