namespace BeStudent.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class ChatsService : IChatsService
    {
        private readonly IDeletableEntityRepository<Chat> chatRepository;
        private readonly IRepository<Message> messageRepository;

        public ChatsService(
            IDeletableEntityRepository<Chat> chatRepository,
            IRepository<Message> messageRepository)
        {
            this.chatRepository = chatRepository;
            this.messageRepository = messageRepository;
        }

        public async Task<int> CreateMessageAsync(string message, string userId, string chatId)
        {
            var newMessage = new Message
            {
                Text = message,
                UserId = userId,
                ChatId = chatId,
            };

            await this.messageRepository.AddAsync(newMessage);
            await this.messageRepository.SaveChangesAsync();
            return newMessage.Id;
        }

        public T GetChat<T>(int semesterId)
        {
            return this.chatRepository
                .All()
                .Where(c => c.SemesterId == semesterId)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetMessage<T>(int messageId)
        {
            return this.messageRepository
                .All()
                .Where(m => m.Id == messageId)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
