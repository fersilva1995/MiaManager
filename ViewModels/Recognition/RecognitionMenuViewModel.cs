using MiaManager.Commands;
using System.Collections.ObjectModel;

namespace MiaManager.ViewModels
{
    public class RecognitionMenuViewModel : BaseViewModel
    {
        public ObservableCollection<DataViewModel> Data { get; set; } = [];

        public RecognizeCommand RecognizeCommand { get; set; }

        public RecognitionMenuViewModel()
        {
            RecognizeCommand = new(this);
        }
    }
}
