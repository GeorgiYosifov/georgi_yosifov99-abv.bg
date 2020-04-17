namespace BeStudent.Web.ViewModels.Course
{
    using System.ComponentModel.DataAnnotations;

    public class CourseCreateInputModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
