namespace BeStudent.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Semester;

    public class SemestersController : BaseController
    {
        private readonly ISemestersService semestersService;

        public SemestersController(ISemestersService semestersService)
        {
            this.semestersService = semestersService;
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var semesterViewModel = this.semestersService.GetDetails<SemesterDetailsViewModel>(id);
            if (semesterViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(semesterViewModel);
        }
    }
}
