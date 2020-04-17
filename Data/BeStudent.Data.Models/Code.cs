namespace BeStudent.Data.Models
{
    using System;

    using BeStudent.Data.Common.Models;

    public class Code : BaseModel<string>
    {
        public Code()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime ExpiresOn { get; set; }

        public string SendTo { get; set; }

        public bool IsUsed { get; set; }
    }
}
