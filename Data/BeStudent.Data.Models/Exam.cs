namespace BeStudent.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Exam : BaseDeletableModel<int>
    {
        public Exam()
        {
            this.SendFiles = new HashSet<SendFile>();
            this.OnlineTests = new HashSet<OnlineTest>();
            this.Students = new HashSet<ApplicationUser>();
            this.Files = new HashSet<File>();
            this.Grades = new HashSet<Grade>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Open { get; set; }

        public DateTime? Close { get; set; }

        public ExamType Type { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<SendFile> SendFiles { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }

        public virtual ICollection<OnlineTest> OnlineTests { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
