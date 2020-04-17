namespace BeStudent.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Answers = new HashSet<Answer>();
        }

        public string Condition { get; set; }

        public int? FileId { get; set; }

        public virtual File File { get; set; }

        public int OnlineTestId { get; set; }

        public virtual OnlineTest OnlineTest { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
