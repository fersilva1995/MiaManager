using MiaManager.Base;
using MiaManager.EventsArgs;
using MiaManager.Services;
using MiaManager.ViewModels;

namespace MiaManager.Commands
{
    public class RemoveDataCommand(DataViewModel vm) : BaseCommand
    {
        public DataViewModel ViewModel { get; set; } = vm;

        public async override void Execute(object? parameter)
        {
            await DataService.Instance.Remove([ViewModel.Id], ViewModel.Type);
            ViewModel.DataEvent?.Invoke(ViewModel, new SelectEventArg() { Id = ViewModel.Id, Name = ViewModel.Name});
        }
    }
}
