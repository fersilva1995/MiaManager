using MiaManager.Base;
using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands
{
    public class LoadInputFilesCommand(TargetViewModel vm) : BaseCommand
    {
        public TargetViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            ViewModel.LoadInputFiles();
        }
    }
}
