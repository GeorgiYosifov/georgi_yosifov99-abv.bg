namespace BeStudent.Web.ViewModels.Exam
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class QuestionCreateInputModel
    {
        public int OnlineTestId { get; set; }

        [Required]
        [Display(Name = "Condition")]
        public string Condition { get; set; }

        [Display(Name = "Image (optional)")]
        public IFormFile Image { get; set; }

        [Display(Name = "How many answers do u want to have?")]
        public int NumberOfAnswers { get; set; }
    }
}
