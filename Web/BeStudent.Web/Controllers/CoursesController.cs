namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Course;
    using BeStudent.Web.ViewModels.Semester;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CoursesController : BaseController
    {
        private readonly ICoursesService coursesService;
        private readonly ISemestersService semestersService;

        public CoursesController(ICoursesService coursesService, ISemestersService semestersService)
        {
            this.coursesService = coursesService;
            this.semestersService = semestersService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("Courses/{courseName}")]
        public IActionResult ByName(string courseName, [FromQuery] int courseId)
        {
            var semesters = this.semestersService.GetAll<SemesterViewModel>(courseName).OrderBy(s => s.Number);

            var viewModel = new CourseWithSemestersViewModel
            {
                Id = courseId,
                Name = courseName,
                Semesters = semesters,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult All()
        {
            var viewModel = new CoursesListViewModel
            {
                Courses = this.coursesService.GetAll<CourseViewModel>(),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.coursesService.CreateAsync(input.Name);
            return this.Redirect("/Courses/All");
        }
    }
}
