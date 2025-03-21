using MiaManager.Services;
using MiaManager.ViewModels;
using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;

using System.IO;
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

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            DataListView.SelectAll();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (DataViewModel item in DataListView.SelectedItems.Cast<DataViewModel>())
                {
                    string extension = ".jpg";
                    string randomName = "User_" + Guid.NewGuid().ToString("N").Substring(6);
                    SaveBitmapImageToFile(item.Image, randomName + extension, new JpegBitmapEncoder());

                }
            }
            catch
            {

            }
        
        }

        public void SaveBitmapImageToFile(BitmapImage bitmapImage, string filePath, BitmapEncoder encoder)
        {
            var frame = BitmapFrame.Create(bitmapImage);
            encoder.Frames.Add(frame);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
    }
}
