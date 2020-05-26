namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    public interface IChatsService
    {
        Task<int> CreateMessageAsync(string message, string userId, string chatId);

        Task<T> GetChat<T>(int semesterId);

        Task<T> GetMessage<T>(int messageId);

        Task<T> GetStudent<T>(string email);

        Task SetUserToActiveOrInActive(string userId, int semesterId, string command);
    }
}
