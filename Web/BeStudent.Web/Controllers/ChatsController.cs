namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Chat;
    using BeStudent.Web.ViewModels.Student;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ChatsController : BaseController
    {
        private readonly IChatsService chatsService;

        public ChatsController(
            IChatsService chatsService)
        {
            this.chatsService = chatsService;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Chat()
        {
            var userName = this.User.FindFirstValue(ClaimTypes.Name);
            var student = await this.chatsService.GetStudent<StudentForChatViewModel>(userName);

            if (student.SemesterNumber == 0)
            {
                this.TempData["message"] = "You can pay your first semester now!";
                return this.RedirectToAction("ChooseCourse", "Payments");
            }

            var semesterId = student.StudentSemesters.LastOrDefault().SemesterId;

            var model = await this.chatsService.GetChat<ChatViewModel>(semesterId);
            model.UserId = student.Id;

            return this.View(model);
        }
    }
}
