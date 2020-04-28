namespace BeStudent.Web.ViewModels.Grade
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Ganss.XSS;

    public class GradeViewModel : IMapFrom<Grade>
    {
        public string StudentId { get; set; }

        public int ExamId { get; set; }

        public int HomeworkId { get; set; }

        public double Mark { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);
    }
}
