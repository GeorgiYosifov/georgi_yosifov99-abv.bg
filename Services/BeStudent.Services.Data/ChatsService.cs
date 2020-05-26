namespace BeStudent.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ChatsService : IChatsService
    {
        private readonly IDeletableEntityRepository<Chat> chatRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> studentRepository;
        private readonly IRepository<Message> messageRepository;

        public ChatsService(
            IDeletableEntityRepository<Chat> chatRepository,
            IDeletableEntityRepository<ApplicationUser> studentRepository,
            IRepository<Message> messageRepository)
        {
            this.chatRepository = chatRepository;
            this.studentRepository = studentRepository;
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

        public async Task<T> GetChat<T>(int semesterId)
        {
            return await this.chatRepository
                .All()
                .Where(c => c.SemesterId == semesterId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<Chat> GetChat(int semesterId)
        {
            return await this.chatRepository
                .All()
                .Where(c => c.SemesterId == semesterId)
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetMessage<T>(int messageId)
        {
            return await this.messageRepository
                .All()
                .Where(m => m.Id == messageId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetStudent<T>(string email)
        {
            return await this.studentRepository
                .All()
                .Where(s => s.Email == email)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task SetUserToActiveOrInActive(string userId, int semesterId, string command)
        {
            var user = await this.studentRepository
                .All()
                .FirstOrDefaultAsync(s => s.Id == userId);

            var chat = await this.GetChat(semesterId);
            if (command == "Add")
            {
                chat.Users.Add(user);
            }
            else if (command == "Remove")
            {
                chat.Users.Remove(user);
            }

            await this.chatRepository.SaveChangesAsync();
        }
    }
}
