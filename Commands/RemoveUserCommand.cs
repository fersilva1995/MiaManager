using MiaManager.Base;
using MiaManager.ViewModels;
using MiaManager.Views.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands
{
    public class RemoveUserCommand(UsersMenuViewModel vm) : BaseCommand
    {
        public UsersMenuViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            ViewModel.Remove();
        }
    }
}
