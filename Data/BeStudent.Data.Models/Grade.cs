namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class Grade : BaseDeletableModel<int>
    {
        public double Mark { get; set; }

        public string Description { get; set; }

        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public int? ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public int? HomeworkId { get; set; }

        public virtual Homework Homework { get; set; }
    }
}
