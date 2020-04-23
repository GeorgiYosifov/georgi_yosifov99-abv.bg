namespace BeStudent.Web.ViewModels.Exam
{
    using System;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class OnlineTestViewModel : IMapFrom<OnlineTest>
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public int QuestionsCount { get; set; }

        public string QuestionId { get; set; }
    }
}
