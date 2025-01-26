using MiaGRPC;
using MiaManager.Base;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.ViewModels;
using System.Collections;
using System.IO;
using System.Windows.Media.Imaging;

namespace MiaManager.Commands
{
    public class LoadDataCommand(DataViewModel vm) : BaseCommand
    {
        public DataViewModel ViewModel { get; set; } = vm;

        public async override void Execute(object? parameter)
        {

            List<ImageData> data =  await DataService.Instance.GetData(ViewModel.Type, [ViewModel.Id]);
            if(data.Count > 0)
            {
                byte[]? bytes = [.. data.First().ImageBytes.ToByteArray()];
                if (bytes != null)
                    ViewModel.Set(bytes);
            }
        }
    }
}
