namespace BeStudent.Web.ViewModels.Home
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class CourseViewModel : IMapFrom<Course>
    {
        public string Name { get; set; }
    }
}
