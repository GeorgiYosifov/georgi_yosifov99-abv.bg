namespace BeStudent.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeStudent.Common;
    using BeStudent.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class DashboardController : AdministrationController
    {
        private readonly IDashBoardService dashBoardService;

        public DashboardController(IDashBoardService dashBoardService)
        {
            this.dashBoardService = dashBoardService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            await this.dashBoardService.SendCodeAsync(email);
            return this.Redirect("/Home/Index");
        }
    }
}
