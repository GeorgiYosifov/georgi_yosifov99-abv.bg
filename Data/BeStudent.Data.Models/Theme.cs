namespace BeStudent.Data.Models
{
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Theme : BaseDeletableModel<int>
    {
        public Theme()
        {
            this.Files = new HashSet<File>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
