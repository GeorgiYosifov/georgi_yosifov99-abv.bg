namespace BeStudent.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class OnlineTest : BaseDeletableModel<int>
    {
        public OnlineTest()
        {
            this.Questions = new HashSet<Question>();
            this.Grades = new HashSet<Grade>();
        }

        public double MinPointsFor3 { get; set; }

        public double Range { get; set; }

        public double MaxPoints { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
