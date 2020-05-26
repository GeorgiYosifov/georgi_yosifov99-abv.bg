namespace BeStudent.Web.ViewModels.Exam
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.File;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Condition { get; set; }

        public FileViewModel File { get; set; }

        public IEnumerable<AnswerViewModel> Answers { get; set; }

        public IEnumerable<DecisionViewModel> Decisions { get; set; }

        public int OnlineTestId { get; set; }

        public DateTime OnlineTestEndTime { get; set; }

        public int OnlineTestDuration { get; set; }

        public string Value { get; set; }

        public long RemainSeconds { get; set; }

        public long SecondsBefore { get; set; }
    }
}
