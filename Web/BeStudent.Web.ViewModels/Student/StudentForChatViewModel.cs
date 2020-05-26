namespace BeStudent.Web.ViewModels.Student
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentForChatViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int SemesterNumber { get; set; }

        public IEnumerable<StudentSemesterViewModel> StudentSemesters { get; set; }
    }
}
