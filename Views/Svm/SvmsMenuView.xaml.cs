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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiaManager.Views.Svm
{
    /// <summary>
    /// Interação lógica para SvmsMenuView.xam
    /// </summary>
    public partial class SvmsMenuView : Page
    {
        public SvmsMenuView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SvmCreateView createView = new();
            createView.ShowDialog();
        }
    }
}
