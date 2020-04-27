namespace BeStudent.Web.ViewModels.Exam
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class ExamSendSolutionInputModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileDescription { get; set; }

        public string SubjectName { get; set; }

        public int ExamId { get; set; }
    }
}
