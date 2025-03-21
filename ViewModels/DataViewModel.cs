using MiaManager.Commands;
using System.IO;
using System.Media;
using System.Windows.Media.Imaging;

namespace MiaManager.ViewModels
{
    public class DataViewModel : BaseViewModel
    {
        private string id = string.Empty;
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string type = string.Empty;
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));

            }
        }

        private BitmapImage image = new();
        public BitmapImage Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private string value = string.Empty;
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public LoadDataCommand LoadDataCommand {  get; set; }
        public RemoveDataCommand RemoveDataCommand { get; set; }

        public EventHandler? DataEvent { get; set; }

        public DataViewModel()
        {
            LoadDataCommand = new(this);
            RemoveDataCommand = new(this);
        }


        public void Set(byte[] bytes)
        {
            if (type == "face_features")
            {
                Value = string.Join(",", bytes);
                return;
            }
            if(type == "audios")
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    SoundPlayer player = new SoundPlayer(ms);
                    player.Play();
                }
            }
            else
            {
                if (bytes.Length == 0)
                    return;

                using var stream = new MemoryStream(bytes);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();

                Image = bitmap;
            }

       
        }
    }
}
