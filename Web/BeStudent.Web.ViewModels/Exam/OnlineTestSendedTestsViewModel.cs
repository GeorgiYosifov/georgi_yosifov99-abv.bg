namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Student;

    public class OnlineTestSendedTestsViewModel : IMapFrom<OnlineTest>
    {
        public int Id { get; set; }

        public IEnumerable<StudentViewModel> Students { get; set; }
    }
}
