namespace BeStudent.Web.ViewModels.Exam
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Ganss.XSS;

    public class DecisionViewModel : IMapFrom<Decision>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public double Points { get; set; }

        public string Type { get; set; }

        public string StudentId { get; set; }
    }
}
