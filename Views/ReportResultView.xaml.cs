using System.Windows;
using MiaManager.Models;
using MiaManager.Services;
using ScottPlot;


namespace MiaManager.Views
{
    /// <summary>
    /// Lógica interna para ReportResultView.xaml
    /// </summary>
    public partial class ReportResultView : Window
    {
        public ReportResultView()
        {
            InitializeComponent();

            var confusionMatrix = ReportResultService.Instance.Selected.ConfusionMatrix;

            // Convert confusion matrix to a 2D array and labels
            var (matrix, classLabels) = ConvertToHeatmapData(confusionMatrix);

            // Plot the heatmap
            var plt = ConfusionMatrixPlot.Plot;
            var hm = plt.Add.Heatmap(matrix);

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    Coordinates coordinates = new(x, y);
                    string cellLabel = matrix[y, x].ToString("0.0");
                    var text = plt.Add.Text(cellLabel, coordinates);
                    text.Alignment = Alignment.MiddleCenter;
                    text.LabelFontSize = 30;
                    text.LabelFontColor = Colors.White;
                }
            }

            plt.Add.ColorBar(hm);

            plt.Axes.Bottom.SetTicks([0, 1], classLabels);
            plt.Axes.Left.SetTicks([0, 1], classLabels);
            plt.Title("Confusion Matrix");
            plt.XLabel("Predicted Labels");
            plt.YLabel("True Labels");

            // Refresh the plot
            ConfusionMatrixPlot.Refresh();
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
