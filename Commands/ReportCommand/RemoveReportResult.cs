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
    public class RemoveReportResult(ReportResultViewModel vm) : BaseCommand
    {
        public ReportResultViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            ReportResultService.Instance.RemoveReport(ViewModel.Id);
        }
    }
}
