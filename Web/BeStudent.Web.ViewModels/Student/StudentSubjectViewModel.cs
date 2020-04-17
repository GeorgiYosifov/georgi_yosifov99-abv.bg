namespace BeStudent.Web.ViewModels.Student
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentSubjectViewModel : IMapFrom<StudentSubject>
    {
        public StudentForSubjectViewModel Student { get; set; }
    }
}
