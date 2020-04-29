namespace BeStudent.Web.ViewModels.Semester
{
    using System.ComponentModel.DataAnnotations;

    public class SemesterCreateInputModel
    {
        public string CourseName { get; set; }

        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Number of semester")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }
    }
}
