using MiaGRPC;
using MiaManager.Base;
using MiaManager.Services;
using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands
{
    public class LoadAllDataCommand(UserViewModel vm) : BaseCommand
    {
        public UserViewModel ViewModel { get; set; } = vm;

        public async override void Execute(object? parameter)
        {
            if(DataService.Instance.Selected.Count > 0)
            {
                List<string> ids = DataService.Instance.Selected.Select(d => d.Id).ToList();
                Dictionary<string, DataViewModel> list = [];
                foreach (DataViewModel viewModel in ViewModel.Data)
                    if(ids.Contains(viewModel.Id)) 
                        list.Add(viewModel.Id, viewModel);    

                List<ImageData> data = await DataService.Instance.GetData(list.First().Value.Type, ids);
                foreach (ImageData imageData in data)
                    list[imageData.Id].Set([.. imageData.ImageBytes]);
            }

      
        }
    }
}
