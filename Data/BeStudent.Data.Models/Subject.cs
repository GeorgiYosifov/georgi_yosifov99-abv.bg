namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Subject : BaseDeletableModel<int>
    {
        public Subject()
        {
            this.StudentSubjects = new HashSet<StudentSubject>();
            this.Exams = new HashSet<Exam>();
            this.Themes = new HashSet<Theme>();
            this.Homeworks = new HashSet<Homework>();
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }

        public virtual ICollection<Theme> Themes { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
