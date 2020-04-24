namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.File;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Condition { get; set; }

        public FileViewModel File { get; set; }

        public IList<AnswerViewModel> Answers { get; set; }

        public int OnlineTestId { get; set; }

        public string Value { get; set; }
    }
}
