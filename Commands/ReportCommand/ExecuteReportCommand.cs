using MiaGRPC;
using MiaManager.Base;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.ViewModels;
using MiaManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Commands.ReportCommand
{
    public class ExecuteReportCommand(ReportViewModel vm) : BaseCommand
    {
        public ReportViewModel ViewModel { get; set; } = vm;

        public override async void Execute(object? parameter)
        {
            Svm? svm = SvmService.Instance.Elements.Where(e => e.Id == ViewModel.SelectedSvm.Id).FirstOrDefault();
            if (svm == null)
                return;
            ReportResult reportResult = new()
            {
                ReportId = ViewModel.Id,
                Name = ViewModel.Name,
                Users = ViewModel.Targets.ToDictionary(t => t.Id, t => t.CurrentFeatureNumber),
                Threshold = ViewModel.Threshold,
                Svms = [svm],
            };



            foreach (TargetViewModel target in ViewModel.Targets)
            {
                if (target.InputFiles.Count <= 0) continue;

                List<Data> data = [];
                foreach (string file in target.InputFiles)
                {
                    reportResult.InputParameters.Add(file, target.Id);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(file);
                    data.Add(new Data { Name = file, Value = [.. imageBytes] });
                }

                List<RecognitionResponse> response = await MiaService.Instance.RecognizeSingle(data, svm, ViewModel.Threshold);
                foreach (RecognitionResponse res in response)
                {
                    if (reportResult.OutputParameters.TryGetValue(res.ImageName, out List<string>? value))
                        value.Add(res.UserId);
                    else
                        reportResult.OutputParameters.Add(res.ImageName, [res.UserId]);

                    if(!reportResult.Scores.ContainsKey(res.ImageName))
                        reportResult.Scores.Add(res.ImageName, []);

                    reportResult.Scores[res.ImageName].Add(new(res.UserId, res.Score));
                }
            }

            ReportResultService.Instance.Add(reportResult);


        }
    }
}
