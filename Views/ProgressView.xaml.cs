using MiaManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiaManager.Views
{
    /// <summary>
    /// Lógica interna para ProgressView.xaml
    /// </summary>
    public partial class ProgressView : Window
    {
        public ProgressView()
        {
            InitializeComponent();
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ProgressService.Instance.Finished)
                Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgressService.Instance.Stop = true;
            ProgressService.Instance.Value = ProgressService.Instance.Max;
            Close();
        }
    }
}
