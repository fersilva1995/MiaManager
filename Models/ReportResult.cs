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

        public Dictionary<string, int> Users { get; set; } = [];
        public List<Svm> Svms { get; set; } = [];

        public double Threshold { get; set; } = 0;

   

        public Dictionary<string, string> InputParameters { get; set; } = [];
        public Dictionary<string, List<string>> OutputParameters { get; set; } = [];
        public Dictionary<string, Dictionary<string, int>> ConfusionMatrix = [];

        public Dictionary<string, List<KeyValuePair<string, double>>> Scores { get; set; } = []; 


        public int TruePositive { get; set; } = 0;
        public int FalsePositive { get; set; } = 0;
        public int TrueNegative { get; set; } = 0;
        public int FalseNegative { get; set; } = 0;

        public double Precision { get; set; } = 0;
        public double Recall { get; set; } = 0;
        public double F1Score { get; set; } = 0;

        public long ReportId { get; set; } = 0;

        public void Load()
        {
            TruePositive = 0;
            FalsePositive = 0;
            TrueNegative = 0;
            FalsePositive = 0;
            ConfusionMatrix.Clear();

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
                                ConfusionMatrix[realUser].Add(predictedUser, 1);
                        }
                    }
                  /*  else
                    {
                        FalseNegative++;
                    }*/
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
                                ConfusionMatrix[realUser].Add(predictedUser, 1);
                        }
                    }
                  /*  else
                    {
                        FalseNegative++;
                    }*/
                }

                // Calculate Precision, Recall, and F1-Score
  
            }

            Precision = TruePositive / (double)(TruePositive + FalsePositive);
            Recall = TruePositive / (double)(TruePositive + FalseNegative);
            F1Score = (2 * Precision * Recall) / (Precision + Recall);

        }

    }
}
