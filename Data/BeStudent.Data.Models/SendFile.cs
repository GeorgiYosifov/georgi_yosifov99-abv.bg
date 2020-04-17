namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class SendFile : BaseModel<int>
    {
        public string CloudinaryFileUri { get; set; }

        public string FileDescription { get; set; }

        public int? HomeworkId { get; set; }

        public virtual Homework Homework { get; set; }

        public int? ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public int? GradeId { get; set; }

        public virtual Grade Grade { get; set; }

        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }
    }
}
