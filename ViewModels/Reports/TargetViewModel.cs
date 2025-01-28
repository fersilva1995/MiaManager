using MiaManager.Commands;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.Views;
using Microsoft.VisualBasic;
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

        private int currentFeatureNumber = 0;
        public int CurrentFeatureNumber
        {
            get
            {
                return currentFeatureNumber;
            }
            set
            {
                currentFeatureNumber = value;
                OnPropertyChanged(nameof(CurrentFeatureNumber));
            }
        }

        public bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public int fileCount = 0;
        public int FileCount
        {
            get
            {
                return fileCount;
            }
            set
            {
                fileCount = value;
                OnPropertyChanged(nameof(FileCount));
            }
        }

        public ObservableCollection<string> InputFiles { get; set; } = [];
        public ObservableCollection<string> OutputFiles { get; set; } = [];

        public LoadInputFilesCommand LoadInputFilesCommand { get; set; }


        public TargetViewModel()
        {
            LoadInputFilesCommand = new(this);
        }


        public void LoadInputFiles()
        {
            //InputFiles.Clear();

            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Title = "Selecione uma imagem",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
                foreach (string file in openFileDialog.FileNames)
                    InputFiles.Add(file);

            FileCount = InputFiles.Count;
        }
    }
}
