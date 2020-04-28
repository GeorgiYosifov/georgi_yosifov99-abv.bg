namespace BeStudent.Web.ViewModels.Semester
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Subject;

    public class SemesterForGradesViewModel : IMapFrom<Semester>
    {
        public string StudentId { get; set; }

        public IList<SubjectForGradesViewModel> Subjects { get; set; }
    }
}