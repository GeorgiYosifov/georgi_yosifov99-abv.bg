namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeStudent.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AnswerCreateInputModel
    {
        //[Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        //[Required]
        [Display(Name = "Points")]
        public double Points { get; set; }

        //[Required]
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
