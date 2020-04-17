namespace BeStudent.Web.ViewModels.Homework
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.File;
    using Ganss.XSS;

    public class HomeworkViewModel : IMapFrom<Homework>
    {
        public int Id { get; set; }

        public string SubjectName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public DateTime Deadline { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }

        public IEnumerable<SendFileViewModel> SendFiles { get; set; }
    }
}
