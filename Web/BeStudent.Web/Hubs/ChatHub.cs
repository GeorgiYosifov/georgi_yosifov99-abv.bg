namespace BeStudent.Web.Hubs
{
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(Roles = "User")]
    public class ChatHub : Hub
    {
        private readonly IChatsService chatsService;

        public ChatHub(IChatsService chatsService)
        {
            this.chatsService = chatsService;
        }

        public async Task Send(MessageCreateInputModel input)
        {
            var messageId = await this.chatsService.CreateMessageAsync(input.Message, input.UserId, input.ChatId);
            var message = this.chatsService.GetMessage<MessageViewModel>(messageId);

            await this.Clients.All.SendAsync(
                "NewMessage",
                message);
        }
    }
}
