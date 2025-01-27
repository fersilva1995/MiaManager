namespace MiaManager.Models
{
    public class Svm
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty ;
        public bool CreateNegative { get; set; } = false;
        public bool CreateUnknown { get; set; } = false;
        public List<string> Users { get; set; } = [];
    }
}
