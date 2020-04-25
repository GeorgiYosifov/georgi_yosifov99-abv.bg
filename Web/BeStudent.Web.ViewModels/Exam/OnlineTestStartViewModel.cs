namespace BeStudent.Web.ViewModels.Exam
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Student;

    public class OnlineTestStartViewModel : IMapFrom<OnlineTest>
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public int QuestionsCount { get; set; }

        public string QuestionId { get; set; }

        public int QuestionNumber { get; set; }

        public IList<StudentViewModel> Students { get; set; }
    }
}
