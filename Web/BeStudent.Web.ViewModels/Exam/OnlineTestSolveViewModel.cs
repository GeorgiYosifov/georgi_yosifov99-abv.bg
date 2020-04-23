namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class OnlineTestSolveViewModel : IMapFrom<OnlineTest>
    {
        public IList<QuestionViewModel> Questions { get; set; }
    }
}
