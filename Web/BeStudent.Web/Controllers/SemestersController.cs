namespace BeStudent.Web.Controllers
{
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Semester;
    using BeStudent.Web.ViewModels.Subject;
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
            return this.RedirectToAction("ByName", "Courses", new { courseName });
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("Semesters/Details")]
        public IActionResult Details([FromQuery] int semesterId)
        {
            var semesterViewModel = this.semestersService.GetDetails<SemesterDetailsViewModel>(semesterId);
            if (semesterViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(semesterViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("Subjects/{subjectId}/AddLector")]
        public IActionResult AddLector(int subjectId, [FromQuery] int semesterId)
        {
            var model = new SubjectAddLectorInputModel
            {
                SemesterId = semesterId,
                SubjectId = subjectId,
            };

            return this.View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Subjects/{subjectId}/AddLector")]
        public async Task<IActionResult> AddLector(int subjectId, [FromQuery] int semesterId, SubjectAddLectorInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var hasCreated = await this.semestersService.AddLectorAsync(subjectId, semesterId, input.Email);
            if (hasCreated == false)
            {
                this.TempData["message"] = "The test has already closed!";
                return this.View(input);
            }

            return this.RedirectToAction("Details", "Semesters", new { semesterId });
        }
    }
}
