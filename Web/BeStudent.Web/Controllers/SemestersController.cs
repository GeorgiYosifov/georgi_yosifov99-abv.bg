namespace BeStudent.Web.Controllers
{
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Semester;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class SemestersController : BaseController
    {
        private readonly ISemestersService semestersService;

        public SemestersController(ISemestersService semestersService)
        {
            this.semestersService = semestersService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("Semesters/Create")]
        public IActionResult Create([FromQuery] string courseName, int courseId)
        {
            var model = new SemesterCreateInputModel
            {
                CourseName = courseName,
                CourseId = courseId,
            };

            return this.View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Semesters/Create")]
        public async Task<IActionResult> Create([FromQuery] string courseName, int courseId, SemesterCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.semestersService.CreateAsync(input.Number, input.Year, courseName, courseId);
            return this.RedirectToAction("ByName", "Courses", new { courseName, courseId });
        }

        [Authorize(Roles = "Administrator")]
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
