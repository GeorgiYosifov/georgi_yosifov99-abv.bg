namespace BeStudent.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Homework : BaseDeletableModel<int>
    {
        public Homework()
        {
            this.Files = new HashSet<File>();
            this.SendFiles = new HashSet<SendFile>();
            this.Grades = new HashSet<Grade>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<SendFile> SendFiles { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
