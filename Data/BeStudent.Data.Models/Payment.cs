namespace BeStudent.Data.Models
{
    using System;

    using BeStudent.Data.Common.Models;

    public class Payment : BaseDeletableModel<string>
    {
        public Payment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        public int SemesterId { get; set; }

        public Semester Semester { get; set; }
    }
}
