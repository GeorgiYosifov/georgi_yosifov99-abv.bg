namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class Answer : BaseDeletableModel<int>
    {
        public bool? IsTrue { get; set; }

        public AnswerType Type { get; set; }

        public double Points { get; set; }

        public string Text { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
