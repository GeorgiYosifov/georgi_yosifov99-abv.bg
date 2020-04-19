namespace BeStudent.Web.ViewModels.Exam
{
    using System;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class OnlineTestForCalendarViewModel : IMapFrom<OnlineTest>
    {
        public DateTime CreatedOn { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
