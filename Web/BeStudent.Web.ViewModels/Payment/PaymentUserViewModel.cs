namespace BeStudent.Web.ViewModels.Payment
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentUserViewModel : IMapFrom<ApplicationUser>
    {
        public string CourseName { get; set; }

        public int SemesterNumber { get; set; }

        public IEnumerable<StudentSemester> StudentSemesters { get; set; }
    }
}
