namespace BeStudent.Web.ViewModels.SendFile
{
    using System.Collections.Generic;

    public class SendFilesListViewModel
    {
        public string SubjectName { get; set; }

        public int HomeworkId { get; set; }

        public int ExamId { get; set; }

        public IEnumerable<SendFileViewModel> SendFiles { get; set; }
    }
}
