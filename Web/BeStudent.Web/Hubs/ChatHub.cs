namespace BeStudent.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Chat;
    using BeStudent.Web.ViewModels.Student;
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

        public async Task Send(string text)
        {
            var email = this.Context.User.Identity.Name;
            var user = await this.chatsService.GetStudent<StudentForChatViewModel>(email);
            var semesterId = user.StudentSemesters.LastOrDefault().SemesterId;
            var chat = await this.chatsService.GetChat<ChatViewModel>(semesterId);

            var messageId = await this.chatsService.CreateMessageAsync(text, user.Id, chat.Id);
            var message = await this.chatsService.GetMessage<MessageViewModel>(messageId);

            var callerTask = this.Clients.Caller.SendAsync("NewMessageToCaller", message);
            var othersTask = this.Clients.Others.SendAsync("NewMessageToOthers", message);

            await Task.WhenAll(callerTask, othersTask);
        }

        public async Task Active()
        {
            var email = this.Context.User.Identity.Name;
            var user = await this.chatsService.GetStudent<StudentForChatViewModel>(email);
            var semesterId = user.StudentSemesters.LastOrDefault().SemesterId;

            await this.chatsService.SetUserToActiveOrInActive(user.Id, semesterId, "Add");
            await this.Clients.All.SendAsync("ActiveStudent", user);
        }

        public async Task InActive()
        {
            var email = this.Context.User.Identity.Name;
            var user = await this.chatsService.GetStudent<StudentForChatViewModel>(email);
            var semesterId = user.StudentSemesters.LastOrDefault().SemesterId;

            await this.chatsService.SetUserToActiveOrInActive(user.Id, semesterId, "Remove");
            await this.Clients.All.SendAsync("InActiveStudent", user.Id);
        }
    }
}
