namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class ExamForCalendarViewModel : IMapFrom<Exam>
    {
        public string Title { get; set; }

        public IList<OnlineTestForCalendarViewModel> OnlineTests { get; set; }
    }
}
