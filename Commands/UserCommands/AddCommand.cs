using MiaManager.Base;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.ViewModels;
using Microsoft.Win32;
using System.Security.Policy;
using System.Windows;

namespace MiaManager.Commands
{
    public class AddCommand(UserViewModel vm) : BaseCommand
    {
        public UserViewModel ViewModel { get; set; } = vm;

        public async override void Execute(object? parameter)
        {
            if (parameter == null) return;

            string? type = parameter.ToString();

            if (type == null) return;

            List<Data> data = [];

            if (type == "images")
            {
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
            }
            else
            {
                data = DataService.Instance.Selected;
            }

            if(await DataService.Instance.Set(data, type))
                ViewModel.LoadData(type);

           /* if (await DataService.Instance.Set(type, name, reference, imageBytes))
                ViewModel.LoadData(type);*/
        }
    }
}
