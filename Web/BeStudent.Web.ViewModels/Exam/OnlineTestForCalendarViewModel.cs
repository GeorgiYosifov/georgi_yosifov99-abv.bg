namespace BeStudent.Web.ViewModels.Exam
{
    using System;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class OnlineTestForCalendarViewModel : IMapFrom<OnlineTest>
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
