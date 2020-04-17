namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Course : BaseDeletableModel<int>
    {
        public Course()
        {
            this.Semesters = new HashSet<Semester>();
        }

        public string Name { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }
    }
}
