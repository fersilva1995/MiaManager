using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiaManager.Commands
{
    public class RefreshUsersCommand(UsersMenuViewModel vm) : ICommand
    {
        public UsersMenuViewModel ViewModel { get; set; } = vm;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            ViewModel.LoadUsers();
        }
    }
}
