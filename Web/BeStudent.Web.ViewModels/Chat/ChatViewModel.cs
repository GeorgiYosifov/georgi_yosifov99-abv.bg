namespace BeStudent.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Student;

    public class ChatViewModel : IMapFrom<Chat>
    {
        public string UserId { get; set; }

        public string Id { get; set; }

        public IEnumerable<StudentForSubjectViewModel> Users { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
