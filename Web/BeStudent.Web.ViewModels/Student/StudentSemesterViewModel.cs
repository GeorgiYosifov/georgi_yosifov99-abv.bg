namespace BeStudent.Web.ViewModels.Student
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentSemesterViewModel : IMapFrom<StudentSemester>
    {
        public int SemesterId { get; set; }

        public StudentViewModel Student { get; set; }
    }
}
