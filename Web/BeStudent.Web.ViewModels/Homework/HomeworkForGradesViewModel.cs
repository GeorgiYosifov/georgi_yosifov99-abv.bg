namespace BeStudent.Web.ViewModels.Homework
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Grade;

    public class HomeworkForGradesViewModel : IMapFrom<Homework>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<GradeViewModel> Grades { get; set; }
    }
}
