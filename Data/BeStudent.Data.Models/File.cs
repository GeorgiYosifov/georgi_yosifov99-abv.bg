namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class File : BaseModel<int>
    {
        public File()
        {
            this.Questions = new HashSet<Question>();
        }

        public string CloudinaryFileUri { get; set; }

        public string FileDescription { get; set; }

        public int? ThemeId { get; set; }

        public virtual Theme Theme { get; set; }

        public int? HomeworkId { get; set; }

        public virtual Homework Homework { get; set; }

        public int? ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
