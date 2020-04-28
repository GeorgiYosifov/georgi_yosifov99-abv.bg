namespace BeStudent.Web.ViewModels.Subject
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Exam;
    using BeStudent.Web.ViewModels.Homework;

    public class SubjectForGradesViewModel : IMapFrom<Subject>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ExamForGradesViewModel> Exams { get; set; }

        public IEnumerable<HomeworkForGradesViewModel> Homeworks { get; set; }

        public IEnumerable<StudentSubject> StudentSubjects { get; set; }
    }
}
