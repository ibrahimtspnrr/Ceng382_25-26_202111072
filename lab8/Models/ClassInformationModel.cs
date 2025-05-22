using System.ComponentModel.DataAnnotations;

namespace lab8.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Student Count is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Student Count must be greater than 0.")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
    }
}
