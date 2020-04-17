namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class StudentSemester : BaseDeletableModel<int>
    {
        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; }
    }
}
