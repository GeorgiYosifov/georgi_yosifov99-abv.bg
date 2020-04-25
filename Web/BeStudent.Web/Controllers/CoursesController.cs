namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Course;
    using BeStudent.Web.ViewModels.Home;
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

        [Authorize]
        [HttpGet("Courses/{name}")]
        public IActionResult ByName(string name)
        {
            var semesters = this.semestersService.GetAll<SemesterViewModel>(name).OrderBy(s => s.Number);

            var viewModel = new CourseWithSemestersViewModel
            {
                Name = name,
                Semesters = semesters,
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult All()
        {
            var viewModel = new CoursesListViewModel
            {
                Courses = this.coursesService.GetAll<CourseViewModel>(),
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
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
