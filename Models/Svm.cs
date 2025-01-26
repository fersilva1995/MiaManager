namespace MiaManager.Models
{
    public class Svm
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty ;
        public List<User> Users { get; set; } = [];
    }
}
