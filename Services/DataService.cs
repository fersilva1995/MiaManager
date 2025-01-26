using MiaGRPC;
using MiaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MiaManager.Services
{
    public class DataService
    {
        #region SINGLETON

        private DataService()
        {

        }

        private static DataService? instance = null;
        public static DataService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        #endregion



        private List<Data> selected = [];
        public List<Data> Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }


        public async Task<List<ImageData>> GetList(string type)
        {
            try
            {
                User? user = UserService.Instance.SelectedUser;
                if (user == null)
                    return [];

                return await MiaService.Instance.ReadIndex(user.Id, type);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<List<ImageData>> GetData(string type, List<string> ids)
        {
            try
            {

                User? user = UserService.Instance.SelectedUser;
                if (user == null)
                    return [];

                List<ImageData> data = (await MiaService.Instance.ReadData(user.Id, type, ids)).Cast<ImageData>().ToList();


                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<bool> Set(List<Data> data, string type)
        {
            try
            {
                User? user = UserService.Instance.SelectedUser;
                if (user == null)
                    return false;

                return await MiaService.Instance.SetData(data, type, user.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public async Task Remove(List<string> ids, string type)
        {
            try
            {
                User? user = UserService.Instance.SelectedUser;
                if (user == null)
                    return;

                await MiaService.Instance.RemoveData(user.Id, type, ids);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
