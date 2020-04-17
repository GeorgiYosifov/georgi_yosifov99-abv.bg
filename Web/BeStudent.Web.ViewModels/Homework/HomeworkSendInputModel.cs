namespace BeStudent.Web.ViewModels.Homework
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class HomeworkSendInputModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileDescription { get; set; }
    }
}
