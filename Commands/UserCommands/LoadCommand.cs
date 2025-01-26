using MiaGRPC;
using MiaManager.Base;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands
{
    public class LoadCommand(UserViewModel vm) : BaseCommand
    {
        public UserViewModel ViewModel { get; set; } = vm;



        public override void Execute(object? parameter)
        {
            if (parameter == null) return;

            string? type = parameter.ToString();

            if (type == null) return;

            ViewModel.LoadData(type);
        }
    }
}
