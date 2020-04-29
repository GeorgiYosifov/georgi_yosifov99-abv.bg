namespace BeStudent.Web.ViewModels.Course
{
    using System.Collections.Generic;

    using BeStudent.Web.ViewModels.Semester;

    public class CourseWithSemestersViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SemesterViewModel> Semesters { get; set; }
    }
}
