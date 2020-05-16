namespace BeStudent.Web.ViewModels.Grade
{
    using System.ComponentModel.DataAnnotations;

    public class GradeSetInputModel
    {
        [Required]
        [Range(2, 6)]
        [Display(Name = "Mark")]
        public double Mark { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string StudentId { get; set; }

        public int HomeworkId { get; set; }

        public int ExamId { get; set; }

        public int? SendFileId { get; set; }

        public string SubjectName { get; set; }
    }
}
