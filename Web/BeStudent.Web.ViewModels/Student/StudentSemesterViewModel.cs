namespace BeStudent.Web.ViewModels.Student
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentSemesterViewModel : IMapFrom<StudentSemester>
    {
        public StudentViewModel Student { get; set; }
    }
}
