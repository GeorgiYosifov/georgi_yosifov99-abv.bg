namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class Decision : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public double? Points { get; set; }

        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
