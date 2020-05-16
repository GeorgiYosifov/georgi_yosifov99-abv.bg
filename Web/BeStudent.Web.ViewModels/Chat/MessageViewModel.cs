namespace BeStudent.Web.ViewModels.Chat
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Ganss.XSS;

    public class MessageViewModel : IMapFrom<Message>
    {
        public string Text { get; set; }

        public string SanitizedText => new HtmlSanitizer().Sanitize(this.Text);

        public string UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
    }
}
