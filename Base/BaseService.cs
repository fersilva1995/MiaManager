using MiaManager.EventsArgs;
using MiaManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Base
{
    public abstract class BaseService
    {
        protected bool run = false;
        protected Thread? service = null;

        public bool Loaded { get; set; } = false;


        public void Start()
        {
            run = true;
            Loaded = false;
            service = new Thread(Run);
            service.Start();
        }

        public void Stop()
        {
            run = false;
            if (service != null && service.IsAlive)
                service.Join();
        }

        public abstract void Run();
    }
}
