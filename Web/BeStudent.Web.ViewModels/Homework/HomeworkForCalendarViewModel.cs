namespace BeStudent.Web.ViewModels.Homework
{
    using System;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class HomeworkForCalendarViewModel : IMapFrom<Homework>
    {
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
