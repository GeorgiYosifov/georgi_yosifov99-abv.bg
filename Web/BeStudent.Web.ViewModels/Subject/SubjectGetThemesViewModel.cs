namespace BeStudent.Web.ViewModels.Subject
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Exam;
    using BeStudent.Web.ViewModels.Homework;
    using BeStudent.Web.ViewModels.Student;
    using BeStudent.Web.ViewModels.Theme;

    public class SubjectGetThemesViewModel : IMapFrom<Subject>
    {
        public string Name { get; set; }

        public IList<StudentSubjectViewModel> StudentSubjects { get; set; }

        public IList<ThemeViewModel> Themes { get; set; }

        public IList<HomeworkViewModel> Homeworks { get; set; }

        public IList<ExamViewModel> Exams { get; set; }
    }
}
