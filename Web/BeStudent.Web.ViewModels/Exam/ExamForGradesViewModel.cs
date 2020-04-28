namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Grade;

    public class ExamForGradesViewModel : IMapFrom<Exam>
    {
        public int Id { get; set; }

        public ExamType Type { get; set; }

        public IEnumerable<GradeViewModel> Grades { get; set; }
    }
}
