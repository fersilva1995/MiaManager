using MiaManager.Base;
using MiaManager.Models;
using System.Text.Json;
using System.Security.Cryptography;

namespace MiaManager.Services
{
    public class ReportResultService : BaseService
    {
        #region SINGLETON

        private ReportResultService()
        {

        }

        private static ReportResultService? instance = null;
        public static ReportResultService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }



        #endregion
        private static readonly object fileLock = new object();
        readonly string filePath = "reportsResults.json";

        public List<ReportResult> Reports = [];
        public ReportResult Selected { get; set; } = new();


        public override void Run()
        {
            while (run)
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        List<ReportResult>? reports = [];
                        lock (fileLock)
                        {

                            string jsonContent = System.IO.File.ReadAllText(filePath);
                            if (!string.IsNullOrEmpty(jsonContent))
                                reports = JsonSerializer.Deserialize<List<ReportResult>>(jsonContent);
                        }

                        if (reports != null)
                        {
                            List<long> removed = Reports.Select(u => u.Id).Except(reports.Select(u => u.Id)).ToList();
                            List<long> added = reports.Select(u => u.Id).Except(Reports.Select(u => u.Id)).ToList();

                            foreach (long key in removed)
                                Reports.RemoveAll(u => u.Id == key);

                            foreach (ReportResult element in reports.Where(u => added.Contains(u.Id)))
                            { 
                                Reports.Add(element);
                                element.Load();
                            }
                        }
                    }
                    else
                    {
                        System.IO.File.Create(filePath);
                    }
                }
                catch
                {

                }

                Thread.Sleep(1000);
            }
        }

        public void Update(ReportResult report)
        {
            lock (fileLock)
            {
                ReportResult? r = Reports.Where(r => r.Id == report.Id).FirstOrDefault();
                if (r != null)
                {
                    int index = Reports.IndexOf(r);
                    Reports[index] = report;
                    WriteReports();
                }
            }
        }

        public void Add(ReportResult report)
        {
            lock (fileLock)
            {
                byte[] buffer = new byte[8];
                RandomNumberGenerator.Fill(buffer);
                long randomId = BitConverter.ToInt64(buffer, 0);

                // Ensure the ID is positive (optional, depending on your use case)
                randomId = Math.Abs(randomId);
                report.Id = randomId;
                Reports.Add(report);
                WriteReports();
            }
        }

        public void RemoveReport(long id)
        {
            lock (fileLock)
            {
                Reports.RemoveAll(r => id == r.Id);
                WriteReports();
            }
        }


        private void WriteReports()
        {
            string jsonContent = JsonSerializer.Serialize(Reports, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, jsonContent);
        }

        internal void SetSelected(long id)
        {
            ReportResult? report = Reports.FirstOrDefault(r => id == r.Id);
            if (report != null)
            {
                Selected = report;
            }
        }
    }
}
