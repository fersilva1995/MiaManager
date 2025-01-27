using MiaManager.Base;
using MiaManager.EventsArgs;
using MiaManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MiaManager.Services
{
    public class SvmService : BaseService
    {
        #region SINGLETON

        private SvmService()
        {

        }

        private static SvmService? instance = null;
        public static SvmService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        #endregion


        private List<Svm> elements = [];
        public List<Svm> Elements { get { return elements; } }

        private Svm? selected = null;
        public Svm? Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                if (value != null)
                    SelectEvent?.Invoke(this, new SelectEventArg() { Id = value.Id, Name = value.Name });
            }
        }

        public EventHandler? SelectEvent { get; set; }

        public void SetSelected(string id)
        {
            var element = elements.Where(u => u.Id == id).FirstOrDefault();
            if (element != null)
            {
                Selected = element;
                SelectEvent?.Invoke(this, new SelectEventArg() { Id = id, Name = element.Name });
            }
        }

        public async Task<bool> Add(string name, List<string> users)
        {
            Svm svm = new()
            {
                Id = "",
                Name = name,
                Users = users,
            };

            string response = await MiaService.Instance.AddSvm(svm);
            return response != string.Empty;
        }

        public override async void Run()
        {
            while (run)
            {
                try
                {
                    Thread.Sleep(2000);

                    string json = await MiaService.Instance.GetSvms();
                    var data = JsonConvert.DeserializeObject<List<Svm>>(json);

                    if (data == null)
                    {
                        elements.Clear();
                        continue;
                    }

                    List<string> removed = elements.Select(u => u.Id).Except(data.Select(u => u.Id)).ToList();
                    List<string> added = data.Select(u => u.Id).Except(elements.Select(u => u.Id)).ToList();

                    foreach (string key in removed)
                        elements.RemoveAll(u => u.Id == key);

                    foreach (Svm element in data.Where(u => added.Contains(u.Id)))
                        elements.Add(element);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Loaded = true;
            }
        }


    }
}
