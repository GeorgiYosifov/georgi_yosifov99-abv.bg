namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Exam : BaseDeletableModel<int>
    {
        public Exam()
        {
            this.SendFiles = new HashSet<SendFile>();
            this.Files = new HashSet<File>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public ExamType Type { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<SendFile> SendFiles { get; set; }

        public virtual ICollection<OnlineTest> OnlineTests { get; set; }
    }
}
