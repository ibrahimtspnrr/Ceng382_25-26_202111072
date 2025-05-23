using System.ComponentModel.DataAnnotations;

namespace lab9.Models
{
    public class Class
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        public string ClassName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Student count must be greater than 0.")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;
    }
}
