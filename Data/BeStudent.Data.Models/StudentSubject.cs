namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class StudentSubject : BaseDeletableModel<int>
    {
        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
