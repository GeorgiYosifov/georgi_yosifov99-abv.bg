namespace BeStudent.Web.ViewModels.Course
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Semester;

    public class CourseViewModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SemesterViewModel> Semesters { get; set; }
    }
}
