namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Semester : BaseDeletableModel<int>
    {
        public Semester()
        {
            this.Subjects = new HashSet<Subject>();
            this.StudentSemesters = new HashSet<StudentSemester>();
            this.Payments = new HashSet<Payment>();
        }

        public int Number { get; set; }

        public SemesterType? SemesterType { get; set; }

        public int Year { get; set; }

        public string CourseName { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<StudentSemester> StudentSemesters { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
