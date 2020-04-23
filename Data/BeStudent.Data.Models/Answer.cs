namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Answer : BaseDeletableModel<int>
    {
        public Answer()
        {
            this.Students = new HashSet<ApplicationUser>();
        }

        public AnswerType Type { get; set; }

        public double? Points { get; set; }

        public string Text { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }
    }
}
