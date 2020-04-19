namespace BeStudent.Web.ViewModels.Calendar
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Exam;
    using BeStudent.Web.ViewModels.Homework;

    public class CalendarViewModel : IMapFrom<Subject>
    {
        public string Name { get; set; }

        public IList<HomeworkForCalendarViewModel> Homeworks { get; set; }

        public IList<ExamForCalendarViewModel> Exams { get; set; }
    }
}
