namespace BeStudent.Web.ViewModels.Semester
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Student;
    using BeStudent.Web.ViewModels.Subject;

    public class SemesterDetailsViewModel : SemesterViewModel,  IMapFrom<Semester>
    {
        public string CourseName { get; set; }

        public int SubjectsCount { get; set; }

        public IList<StudentSemesterViewModel> StudentSemesters { get; set; }

        public IList<SubjectViewModel> Subjects { get; set; }
    }
}
