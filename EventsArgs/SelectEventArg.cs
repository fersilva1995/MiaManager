using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.EventsArgs
{
    class SelectEventArg : EventArgs
    {
        public object Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
