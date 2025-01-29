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
    public class ExecuteSvmCommand(SvmViewModel vm) : BaseCommand
    {
        public SvmViewModel ViewModel { get; set; } = vm;

        public override async void Execute(object? parameter)
        {
            await MiaService.Instance.TrainSvm(ViewModel.Id);
        }
    }
}
