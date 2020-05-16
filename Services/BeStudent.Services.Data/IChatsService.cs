using System.Threading.Tasks;

namespace BeStudent.Services.Data
{
    public interface IChatsService
    {
        Task<int> CreateMessageAsync(string message, string userId, string chatId);

        T GetChat<T>(int semesterId);

        T GetMessage<T>(int messageId);
    }
}
