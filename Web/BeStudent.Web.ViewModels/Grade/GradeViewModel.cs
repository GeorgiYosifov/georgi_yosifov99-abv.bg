namespace BeStudent.Web.ViewModels.Grade
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class GradeViewModel : IMapFrom<Grade>
    {
        public int Mark { get; set; }

        public string Description { get; set; }
    }
}
