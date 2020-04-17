namespace BeStudent.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;
    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Subject;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SubjectsController : BaseController
    {
        private readonly ISubjectsService subjectsService;
        private readonly UserManager<ApplicationUser> userManager;

        public SubjectsController(
            ISubjectsService subjectsService,
            UserManager<ApplicationUser> userManager)
        {
            this.subjectsService = subjectsService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("Subjects/Create/{id}")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Subjects/Create/{id}")]
        public async Task<IActionResult> Create(int id, SubjectCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.subjectsService.CreateAsync(id, input.Name, input.Emails);
            return this.Redirect($"/Semesters/Details/{id}");
        }

        [Authorize(Roles = "Lector")]
        public IActionResult All()
        {
            var lectorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = new SubjectsListViewModel
            {
                Subjects = this.subjectsService
                    .GetAll<SubjectForAllViewModel>(lectorId),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Lector")]
        public IActionResult Themes(string subjectName)
        {
            var subjectViewModel = this.subjectsService.GetThemes<SubjectGetThemesViewModel>(subjectName);
            if (subjectViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(subjectViewModel);
        }
    }
}
