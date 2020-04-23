namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeStudent.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

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

        [Display(Name = "Answer Type")]
        public AnswerType Type { get; set; }

        public List<SelectListItem> AnswerTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Radio buttons", Value = "RadioButtons" },
            new SelectListItem { Text = "Input field with small content", Value = "InputFieldUp20Chars" },
            new SelectListItem { Text = "Input field with large content", Value = "InputFieldTiny" },
        };
    }
}
