namespace BeStudent.Web.ViewModels.Subject
{
    using BeStudent.Data.Models;

    using BeStudent.Services.Mapping;

    public class SubjectForAllViewModel : IMapFrom<Subject>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
