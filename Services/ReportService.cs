using MiaManager.Base;
using MiaManager.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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



        #endregion
        private static readonly object fileLock = new object();
        readonly string filePath = "reports.json";

        public List<Report> Reports = [];

        public override void Run()
        {
            while (run)
            {
                if (System.IO.File.Exists(filePath))
                {
                    List<Report>? reports = [];
                    lock (fileLock)
                    {

                        string jsonContent = System.IO.File.ReadAllText(filePath);
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


        private  void WriteReports()
        {
            string jsonContent = JsonSerializer.Serialize(Reports, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, jsonContent);
        }




    }
}
