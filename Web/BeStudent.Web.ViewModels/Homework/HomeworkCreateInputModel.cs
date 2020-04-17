namespace BeStudent.Web.ViewModels.Homework
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class HomeworkCreateInputModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile File { get; set; }

        public string FileDescription { get; set; }

        [Required]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }
    }
}
