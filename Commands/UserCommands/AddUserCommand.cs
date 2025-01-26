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
    public class AddUserCommand(UserViewModel vm) : BaseCommand
    {
        public UserViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            _ = UserService.Instance.AddUser(ViewModel.Name);
        }
    }
}
