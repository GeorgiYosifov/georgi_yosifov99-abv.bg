namespace BeStudent.Web.ViewModels.Exam
{
    using System.ComponentModel.DataAnnotations;

    public class AnswerCreateInputModel
    {
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Points")]
        public double Points { get; set; }
    }
}
