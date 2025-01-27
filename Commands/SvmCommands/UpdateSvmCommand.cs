using MiaManager.Base;
using MiaManager.Services;
using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands.SvmCommands
{
    public class UpdateSvmCommand(SvmViewModel vm) : BaseCommand
    {
        public SvmViewModel ViewModel { get; set; } = vm;

        public async override void Execute(object? parameter)
        {
            await MiaService.Instance.PutSvm(new()
            {
                Id = ViewModel.Id,
                Name = ViewModel.Name,
                CreateNegative = ViewModel.CreateNegative,
                CreateUnknown = ViewModel.CreateUnknown,
                Users = ViewModel.Users.Where(u => u.IsSelected == true).Select(u => u.Id).ToList(),
            });
        }
    }
}
