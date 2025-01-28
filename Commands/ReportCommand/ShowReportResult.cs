using MiaManager.Base;
using MiaManager.Services;
using MiaManager.ViewModels;
using MiaManager.Views;

namespace MiaManager.Commands
{
    public class ShowReportResult(ReportResultViewModel vm) : BaseCommand
    {
        public ReportResultViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            ReportResultService.Instance.SetSelected(ViewModel.Id);
            ReportResultView view = new();
            view.Show();
        }
    }
}
