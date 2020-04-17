namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeStudent.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ExamCreateInputModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "File")]
        public IFormFile File { get; set; }

        [Display(Name = "File Name")]
        public string FileDescription { get; set; }

        [Required]
        [Display(Name = "Choose exam type")]
        public ExamType ExamType { get; set; }

        public List<SelectListItem> ExamTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Online Test", Value = "OnlineTest" },
            new SelectListItem { Text = "Online Exam", Value = "OnlineExam" },
            new SelectListItem { Text = "Present Exam", Value = "PresentExam" },
        };
    }
}
