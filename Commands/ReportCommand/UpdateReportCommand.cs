using MiaManager.Base;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands.ReportCommand
{
    public class UpdateReportCommand(ReportViewModel vm) : BaseCommand
    {
        public ReportViewModel ViewModel { get; set; } = vm;

        public override void Execute(object? parameter)
        {
            Report report = new()
            {
                Id = ViewModel.Id,
                Name = ViewModel.Name,
                Svm = SvmService.Instance.Elements.Where(e => e.Id == ViewModel.SelectedSvm.Id).First(),
                Threshold = ViewModel.Threshold,
                Targets = ViewModel.Targets.Select(t => new Target()
                {
                    Id = t.Id,
                    Name = t.Name,
                    CurrentFeatureNumber = t.CurrentFeatureNumber,
                    InputFiles = [.. t.InputFiles],
                    OutputFiles = [ .. t.OutputFiles],
                }).ToList()
            };

            ReportService.Instance.Update(report);


        }
    }
}
