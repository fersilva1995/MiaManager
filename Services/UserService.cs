using MiaManager.EventsArgs;
using MiaManager.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Services
{
    public class UserService
    {
        #region SINGLETON

        private UserService()
        {

        }

        private static UserService? instance = null;
        public static UserService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }


        #endregion

        private bool run = false;
        private Thread? service = null;



        private List<User> users = [];
        public List<User> Users { get { return users; } }

        private User? selectedUser = null;
        public User? SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                if (value != null)
                    SelectUserEvent?.Invoke(this, new SelectEventArg() { Id = value.Id, Name = value.Name });
            }
        }

        public EventHandler? SelectUserEvent { get; set; }

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

        public async void Run()
        {
            while (run)
            {
                try
                {
                    Thread.Sleep(2000);

                    string json = await MiaService.Instance.GetUsers();
                    var data = JsonConvert.DeserializeObject<List<User>>(json);

                    if (data == null)
                    {
                        users.Clear();
                        continue;
                    }

                    List<string> removed = users.Select(u => u.Id).Except(data.Select(u => u.Id)).ToList();
                    List<string> added = data.Select(u => u.Id).Except(users.Select(u => u.Id)).ToList();

                    foreach (string key in removed)
                        users.RemoveAll(u => u.Id == key);

                    foreach (User user in data.Where(u => added.Contains(u.Id)))
                        users.Add(user);


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Loaded = true;
            }
        }

        public void SetSelectedUser(string id)
        {
            var user = users.Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
                SelectedUser = user;
        }

        public async Task<bool> AddUser(string name)
        {
            User user = new()
            {
                Id = "",
                Name = name,
            };

            string response = await MiaService.Instance.AddUser(user);
            return response != string.Empty;
        }

        public async void Remove()
        {
            if(SelectedUser != null)
                await MiaService.Instance.RemoveUser(SelectedUser);
        }
    }
}
