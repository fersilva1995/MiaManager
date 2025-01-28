using MiaManager.Base;
using MiaManager.ViewModels;

namespace MiaManager.Commands.MiaCommands
{
    public class RefreshReportsCommand(ReportMenuViewModel vm) : BaseCommand
    {
        public ReportMenuViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            ViewModel.Load();
        }
    }
}
