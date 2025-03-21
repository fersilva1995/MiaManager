using System.Windows;
using MiaManager.Models;
using MiaManager.Services;
using ScottPlot;
using ScottPlot.Plottables;


namespace MiaManager.Views
{
    /// <summary>
    /// Lógica interna para ReportResultView.xaml
    /// </summary>
    public partial class ReportResultView : Window
    {

        Dictionary<double, double> precision = [];
        Dictionary<double, double> recall = [];
        Dictionary<double, double> f1 = [];

        List<ReportResult> results = [];


        public double[,] Transpose(double[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            double[,] result = new double[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        public void AnaliseData()
        {
            for(double treshold = 0; treshold <= 100; treshold = treshold + 0.5)
            {
                ReportResult reportResult = new();
                reportResult.Threshold = treshold;
                reportResult.InputParameters = ReportResultService.Instance.Selected.InputParameters;
                var scores = ReportResultService.Instance.Selected.Scores;

                foreach(var input in reportResult.InputParameters)
                {
                    if (scores.TryGetValue(input.Key, out List<KeyValuePair<string, double>>? value))
                    {
                        reportResult.OutputParameters.Add(input.Key, []);
                        foreach(KeyValuePair<string, double> detection in value)
                        {
                            string user = detection.Key;
                            if (detection.Key != "unknown" && detection.Value < (treshold/100))
                                user = "unknown";

                            reportResult.OutputParameters[input.Key].Add(user);
                        }
                    }
                }

                reportResult.Load();
                results.Add(reportResult);
                precision.Add(treshold, reportResult.Precision);
                recall.Add(treshold, reportResult.Recall);
                f1.Add(treshold, reportResult.F1Score);
            }

            ReportResult r = results.MaxBy(x => x.F1Score);
            Precision.Text = r.Precision.ToString();
            Recall.Text = r.Recall.ToString();
            F1.Text = r.F1Score.ToString();
        }

        public ReportResultView()
        {
            InitializeComponent();

            AnaliseData();

            var confusionMatrix = results.OrderByDescending(d => d.F1Score).First().ConfusionMatrix;
            // Convert confusion matrix to a 2D array and labels
            var (matrix, classLabels) = ConvertToHeatmapData(confusionMatrix);
            double[,] transposedMatrix = Transpose(matrix);

            // Plot the heatmap
            var plt = ConfusionMatrixPlot.Plot;
            var hm = plt.Add.Heatmap(matrix);

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    Coordinates coordinates = new(x, matrix.GetLength(0)-y-1);
                    string cellLabel = matrix[y, x].ToString("0.0");
                    var text = plt.Add.Text(cellLabel, coordinates);
                    text.Alignment = Alignment.MiddleCenter;
                    text.LabelFontSize = 30;
                    text.LabelFontColor = Colors.White;
                }
            }
 

            plt.Axes.Bottom.SetTicks([0,1], classLabels);
            plt.Axes.Left.SetTicks([1,0], classLabels);
            plt.Title("Matriz confusão");
            plt.XLabel("Valores previstos");
            plt.YLabel("Valores reais");

            hm.Colormap = new ScottPlot.Colormaps.Turbo();
            ConfusionMatrixPlot.Plot.Add.ColorBar(hm);
            // Refresh the plot
            ConfusionMatrixPlot.Refresh();

            var pp = PrecisionPlot.Plot.Add.Scatter(precision.Keys.ToArray(), precision.Values.ToArray());
            pp.Smooth = true;
            pp.LineWidth = 2;
            pp.MarkerSize = 0;
            

            PrecisionPlot.Plot.Title("Precisão x Confiança");
            PrecisionPlot.Plot.XLabel("Confiança");
            PrecisionPlot.Plot.YLabel("Precisão");
            PrecisionPlot.Refresh();

            var rp = RecallPlot.Plot.Add.Scatter(recall.Keys.ToArray(), recall.Values.ToArray());

            rp.Smooth = true;
            rp.LineWidth = 2;
           

            RecallPlot.Plot.Title("Recall x Confiança");
            RecallPlot.Plot.XLabel("Confiança");
            RecallPlot.Plot.YLabel("Recall");
            rp.MarkerSize = 0;
            RecallPlot.Refresh();

            var fp = F1Plot.Plot.Add.Scatter(f1.Keys.ToArray(), f1.Values.ToArray());

            fp.Smooth = true;
            fp.LineWidth = 2;
            fp.MarkerSize = 0;
   

            F1Plot.Plot.Title("F1 Score x Confiança");
            F1Plot.Plot.XLabel("Confiança");
            F1Plot.Plot.YLabel("F1 Score");
            F1Plot.Refresh();


            ConfusionMatrixPlot.Plot.SavePng("plot1.png", 1280,1024);
            PrecisionPlot.Plot.SavePng("plot2.png", 1280, 1024);
            RecallPlot.Plot.SavePng("plot3.png", 1280, 1024);
            F1Plot.Plot.SavePng("plot4.png", 1280, 1024);
        }

        static (double[,], string[]) ConvertToHeatmapData(Dictionary<string, Dictionary<string, int>> confusionMatrix)
        {
            var labels = new List<string>(confusionMatrix.Keys);
            int n = labels.Count;
            var matrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    string actual = labels[i];
                    string predicted = labels[j];
                    if (confusionMatrix[actual].ContainsKey(predicted))
                        matrix[i, j] = confusionMatrix[actual][predicted];
                    else
                        matrix[i, j] = 0;
                }
            }

            return (matrix, labels.ToArray());
        }
    }
}
