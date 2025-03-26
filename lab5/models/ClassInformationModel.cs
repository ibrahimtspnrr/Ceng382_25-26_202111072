using System.ComponentModel.DataAnnotations;

namespace YourProjectNamespace.Models
{
    public class ClassInformationModel
    {
        private static int _counter = 1; // Id için otomatik artan sayaç

        public ClassInformationModel()
        {
            Id = _counter++;
        }

        public int Id { get; private set; } // Otomatik artan Id

        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Student Count is required")]
        [Range(1, 500, ErrorMessage = "Student Count must be between 1 and 500")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
