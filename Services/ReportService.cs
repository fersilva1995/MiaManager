using MiaManager.Base;
using MiaManager.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;
using MiaManager.ViewModels;
using Google.Protobuf.WellKnownTypes;
using MiaManager.EventsArgs;

namespace MiaManager.Services
{
    public class ReportService : BaseService
    {
        #region SINGLETON

        private ReportService()
        {

        }

        private static ReportService? instance = null;
        public static ReportService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        public Report Selected { get; set; } = new();

        public EventHandler? SelectEvent { get; set; }

        #endregion
        private static readonly object fileLock = new object();
        readonly string filePath = "reports.json";

        public List<Report> Reports = [];

        public override void Run()
        {
            while (run)
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        List<Report>? reports = [];
                        lock (fileLock)
                        {

                            string jsonContent = System.IO.File.ReadAllText(filePath);
                            if (!string.IsNullOrEmpty(jsonContent))
                                reports = JsonSerializer.Deserialize<List<Report>>(jsonContent);
                        }

                        if (reports != null)
                        {
                            List<long> removed = Reports.Select(u => u.Id).Except(reports.Select(u => u.Id)).ToList();
                            List<long> added = reports.Select(u => u.Id).Except(Reports.Select(u => u.Id)).ToList();

                            foreach (long key in removed)
                                Reports.RemoveAll(u => u.Id == key);

                            foreach (Report element in reports.Where(u => added.Contains(u.Id)))
                                Reports.Add(element);
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

        public void Update(Report report)
        {
            lock (fileLock)
            {
                Report? r = Reports.Where(r => r.Id ==  report.Id).FirstOrDefault();
                if (r != null)
                {
                    int index = Reports.IndexOf(r);
                    Reports[index] = report;
                    WriteReports();
                }
            }
        }

        public void Add(Report report)
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

        public void RemoveReport(Report report)
        {
            lock (fileLock)
            {
                Reports.RemoveAll(r => report.Id == r.Id);
                WriteReports();
            }
        }

        public void SetSelected(long reportId)
        {

            Report? selected = Reports.Where(r => r.Id == reportId).FirstOrDefault();
            if(selected != null)
            {
                Selected = selected;
                SelectEvent?.Invoke(this, new SelectEventArg() { Id = selected.Id, Name = selected.Name });
            }
        }

        private  void WriteReports()
        {
            string jsonContent = JsonSerializer.Serialize(Reports, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, jsonContent);
        }
    }
}
