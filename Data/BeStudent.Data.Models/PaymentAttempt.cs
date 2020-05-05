namespace BeStudent.Data.Models
{
    using System;

    using BeStudent.Data.Common.Models;

    public class PaymentAttempt : BaseModel<string>
    {
        public PaymentAttempt()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public decimal Price { get; set; }

        public string StudentId { get; set; }

        public int SemesterId { get; set; }
    }
}
