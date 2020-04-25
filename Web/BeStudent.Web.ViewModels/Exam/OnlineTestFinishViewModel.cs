namespace BeStudent.Web.ViewModels.Exam
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class OnlineTestFinishViewModel : IMapFrom<OnlineTest>
    {
        public string StudentId { get; set; }

        public double Points { get; set; }

        public double? Mark { get; set; }

        public DateTime Send { get; set; }

        public int Id { get; set; }

        public double MinPointsFor3 { get; set; }

        public double Range { get; set; }

        public double MaxPoints { get; set; }

        public DateTime EndTime { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}
