namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;
    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ChatsController : BaseController
    {
        private readonly IChatsService chatsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatsController(
            IChatsService chatsService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatsService = chatsService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Chat()
        {
            var applicationUser = await this.userManager.GetUserAsync(this.User);
            if (applicationUser.SemesterNumber == 0)
            {
                this.TempData["message"] = "You can pay your first semester now!";
                return this.RedirectToAction("ChooseCourse", "Payments");
            }

            var semesterId = applicationUser.StudentSemesters.LastOrDefault().SemesterId;

            var model = this.chatsService.GetChat<ChatViewModel>(semesterId);
            model.UserId = applicationUser.Id;

            return this.View(model);
        }
    }
}
