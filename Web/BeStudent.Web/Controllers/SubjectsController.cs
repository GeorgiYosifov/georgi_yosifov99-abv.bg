namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;
    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Calendar;
    using BeStudent.Web.ViewModels.Exam;
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
        [HttpGet("Subjects/Create")]
        public IActionResult Create([FromQuery] int semesterId)
        {
            var model = new SubjectCreateInputModel
            {
                SemesterId = semesterId,
            };

            return this.View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Subjects/Create")]
        public async Task<IActionResult> Create([FromQuery] int semesterId, SubjectCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.subjectsService.CreateAsync(semesterId, input.Name, input.Price, input.Emails);
            return this.RedirectToAction("Details", "Semesters", new { semesterId });
        }

        [Authorize(Roles = "Lector, User")]
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

        [Authorize(Roles = "Lector, User")]
        public IActionResult Themes(string subjectName)
        {
            var subjectViewModel = this.subjectsService.GetThemes<SubjectGetThemesViewModel>(subjectName);
            if (subjectViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(subjectViewModel);
        }

        [Authorize(Roles = "User, Lector")]
        [HttpGet("Subjects/{subjectName}/Calendar")]
        public IActionResult Calendar(string subjectName, [FromQuery] int month)
        {
            var calendarViewModel = this.subjectsService.FillCalendar<CalendarViewModel>(subjectName);
            if (calendarViewModel == null)
            {
                return this.NotFound();
            }

            var model = new CalendarViewModel
            {
                Name = calendarViewModel.Name,
                Homeworks = calendarViewModel.Homeworks.Where(h => h.CreatedOn.Month == month).ToList(),
                Exams = calendarViewModel.Exams
                        .Select(e => new ExamForCalendarViewModel {
                            Title = e.Title,
                            OnlineTests = e.OnlineTests.Where(t => t.StartTime.Month == month).ToList(),
                        }).ToList(),
            };

            return this.View(model);
        }
    }
}
