using MiaManager.Commands;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace MiaManager.ViewModels
{
    public class TargetViewModel : BaseViewModel
    {
        public int CurrentFeatureCount { get; set; } = 0;

        private string id = string.Empty;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<string> InputFiles { get; set; } = [];

        public LoadInputFilesCommand LoadInputFilesCommand { get; set; }

        public TargetViewModel()
        {
            LoadInputFilesCommand = new(this);
        }


        public void LoadInputFiles()
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Title = "Selecione uma imagem",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
                InputFiles = [.. openFileDialog.FileNames];
        }
    }
}
