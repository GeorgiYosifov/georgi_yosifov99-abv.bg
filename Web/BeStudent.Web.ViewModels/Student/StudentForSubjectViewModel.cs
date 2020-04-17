namespace BeStudent.Web.ViewModels.Student
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentForSubjectViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
