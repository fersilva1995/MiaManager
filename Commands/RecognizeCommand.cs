using MiaGRPC;
using MiaManager.Base;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace MiaManager.Commands
{
    public class RecognizeCommand(RecognitionMenuViewModel vm) : BaseCommand
    {
        public RecognitionMenuViewModel ViewModel { get; set; } = vm;

        public async override void Execute(object? parameter)
        {
            List<Data> data = [];

            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Title = "Selecione uma imagem",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string[] selectedFiles = openFileDialog.FileNames;
                    foreach (string file in selectedFiles)
                    {
                        string fileName = file;
                        byte[] imageBytes = System.IO.File.ReadAllBytes(fileName);
                        string name = fileName.Split("\\").Last().Split(".").First();
                        data.Add(new Data { Name = name, Value = [.. imageBytes] });
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load the image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            List<RecognitionResponse> responseData = await MiaService.Instance.Recognize(data);
            if (responseData.Count > 0)
            {
                foreach(RecognitionResponse response in responseData)
                {
                    byte[]? bytes = [.. response.Image.ToByteArray()];
                    if (bytes != null)
                    {
                        DataViewModel dataViewModel = new()
                        {
                            Id = response.UserId,
                            Name = response.Name,
                        };

                        ViewModel.Data.Add(dataViewModel);
                        dataViewModel.Set(bytes);
                    }
                }
           
            }
        }
    }
}
