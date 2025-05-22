namespace lab8.Models
{
    public class ClassInformationTable
    {
        public int Id { get; set; } // Tabloda görünmeyebilir ama silme/edit için gereklidir
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public string Description { get; set; }
    }
}
