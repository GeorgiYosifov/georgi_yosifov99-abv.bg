namespace BeStudent.Web.ViewModels.Exam
{
    using System.ComponentModel.DataAnnotations;

    public class SetPointsInputModel
    {
        [Required]
        [Display(Name = "Set points")]
        public double Points { get; set; }
    }
}
