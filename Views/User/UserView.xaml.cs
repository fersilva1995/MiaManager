using MiaManager.Services;
using MiaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MiaManager.Views
{
    /// <summary>
    /// Interação lógica para UserView.xam
    /// </summary>
    public partial class UserView : Page
    {
        public UserView()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<DataViewModel> removed = e.RemovedItems.Cast<DataViewModel>().ToList();
            List<DataViewModel> added = e.AddedItems.Cast<DataViewModel>().ToList();


            DataService.Instance.Selected.AddRange(added.Select(d => new Models.Data()
            {
                Id = d.Id,
                Name = d.Name,
            }));
            DataService.Instance.Selected.RemoveAll(d => removed.Select(r => r.Id).Contains(d.Id));
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            DataListView.SelectedItems.Clear(); 
        }
    }
}
