namespace BeStudent.Web.ViewModels.Student
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentForGradesViewModel : IMapFrom<ApplicationUser>
    {
        public IEnumerable<StudentSemester> StudentSemesters { get; set; }
    }
}
