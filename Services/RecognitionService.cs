using MiaGRPC;
using MiaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Services
{
    public class RecognitionService
    {
        #region SINGLETON

        private RecognitionService()
        {

        }

        private static RecognitionService? instance = null;
        public static RecognitionService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        #endregion


        /*private List<Data> selected = [];
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
        }*/

       /* public async Task<List<RecognitionResponse>> Recognize(List<Data> data)
        {
            List<RecognitionResponse> response = await MiaService.Instance.Recognize(data);
            foreach(RecognitionResponse responseItem in response)
            {
                result.Add(new()
                {
                    Id = responseItem.UserId,
                    Name = responseItem.Name,
                    Value = responseItem.Image.ToList()
                });
            }
        }*/
    }
}
