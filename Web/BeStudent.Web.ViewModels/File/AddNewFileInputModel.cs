namespace BeStudent.Web.ViewModels.File
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class AddNewFileInputModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }

        [Required]
        [Display(Name = "File description")]
        public string FileDescription { get; set; }

        public string SubjectName { get; set; }

        public int HomeworkId { get; set; }

        public int ExamId { get; set; }

        public int ThemeId { get; set; }
    }
}
