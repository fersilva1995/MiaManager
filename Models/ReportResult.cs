using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MiaManager.Models
{
    public class ReportResult
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public List<User> Users { get; set; } = [];
        public List<Svm> Svms { get; set; } = [];

        public int TruePositive { get; set; } = 0;
        public int FalsePositive { get; set; } = 0;
        public int TrueNegative { get; set; } = 0;
        public int FalseNegative { get; set; } = 0;

        public double Precision {  get; set; } = 0;
        public double Recall { get; set; } = 0;
        public double F1Score { get; set; } = 0;

        public Dictionary<string, string> InputParameters { get; set; } = [];
        public Dictionary<string, List<string>> OutputParameters { get; set; } = [];
        public Dictionary<string, Dictionary<string, int>> ConfusionMatrix = [];

        public long ReportId { get; set; } = 0;

        public void Load()
        {
            TruePositive = 0;
            FalsePositive = 0;
            TrueNegative = 0;
            FalsePositive = 0;

            foreach (var input in InputParameters)
            {
                string fileName = input.Key;
                string realUser = input.Value;

                if (!ConfusionMatrix.ContainsKey(realUser))
                    ConfusionMatrix.Add(realUser, []);

                if (realUser != "unknown")
                {
                    if (OutputParameters.ContainsKey(fileName))
                    {
                        foreach (string predictedUser in OutputParameters[fileName])
                        {
                            if (predictedUser == realUser)
                            {
                                TruePositive++;
                            }
                            else
                            {
                                if (predictedUser == "unknown")
                                    FalseNegative++;
                                else
                                    FalsePositive++;
                            }

                            if (ConfusionMatrix[realUser].ContainsKey(predictedUser))
                                ConfusionMatrix[realUser][predictedUser]++;
                            else
                                ConfusionMatrix[realUser].Add(predictedUser, 0);
                        }
                    }
                    else
                    {
                        FalseNegative++;
                    }
                }
                else
                {
                    if (OutputParameters.ContainsKey(fileName))
                    {
                        foreach (string predictedUser in OutputParameters[fileName])
                        {
                            if (predictedUser == realUser)
                                TrueNegative++;
                            else
                                FalsePositive++;

                            if (ConfusionMatrix[realUser].ContainsKey(predictedUser))
                                ConfusionMatrix[realUser][predictedUser]++;
                            else
                                ConfusionMatrix[realUser].Add(predictedUser, 0);
                        }
                    }
                    else
                    {
                        FalseNegative++;
                    }
                }

                // Calculate Precision, Recall, and F1-Score
  
            }

            Precision = TruePositive / (double)(TruePositive + FalsePositive);
            Recall = TruePositive / (double)(TruePositive + FalseNegative);
            F1Score = (2 * Precision * Recall) / (Precision + Recall);

        }

    }
}
