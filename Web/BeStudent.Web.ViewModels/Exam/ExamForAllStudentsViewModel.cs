namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Grade;
    using BeStudent.Web.ViewModels.Student;

    public class ExamForAllStudentsViewModel : IMapFrom<Exam>
    {

        public string SubjectName { get; set; }

        public int Id { get; set; }

        public IEnumerable<StudentViewModel> Students { get; set; }

        public IEnumerable<GradeViewModel> Grades { get; set; }
    }
}
