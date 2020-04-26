namespace BeStudent.Web.ViewModels.Exam
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OnlineTestCreateInputModel
    {
        public string SubjectName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "How many questions do you want to make?")]
        public int QuestionsCount { get; set; }

        [Required]
        [Display(Name = "Minimum points for grade 3")]
        public double MinPointsFor3 { get; set; }

        [Required]
        [Display(Name = "Range between grades")]
        public double Range { get; set; }

        [Required]
        [Display(Name = "Max points")]
        public double MaxPoints { get; set; }

        [Required]
        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Duration time in minutes")]
        public int Duration { get; set; }
    }
}
