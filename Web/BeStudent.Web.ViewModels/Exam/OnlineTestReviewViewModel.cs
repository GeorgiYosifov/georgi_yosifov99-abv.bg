namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class OnlineTestReviewViewModel : IMapFrom<OnlineTest>
    {
        public string StudentId { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}
