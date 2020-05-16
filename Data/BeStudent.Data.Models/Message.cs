namespace BeStudent.Data.Models
{
    using BeStudent.Data.Common.Models;

    public class Message : BaseModel<int>
    {
        public string Text { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ChatId { get; set; }

        public virtual Chat Chat { get; set; }
    }
}
