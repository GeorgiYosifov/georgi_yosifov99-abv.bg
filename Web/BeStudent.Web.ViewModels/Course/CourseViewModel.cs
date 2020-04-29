namespace BeStudent.Web.ViewModels.Course
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class CourseViewModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
