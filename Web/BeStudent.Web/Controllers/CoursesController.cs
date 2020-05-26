namespace BeStudent.Web.Controllers
{
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CoursesController : BaseController
    {
        private readonly ICoursesService coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            this.coursesService = coursesService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("Courses/{courseName}")]
        public async Task<IActionResult> ByName(string courseName)
        {
            var viewModel = await this.coursesService.ByName<CourseViewModel>(courseName);

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> All()
        {
            var viewModel = new CoursesListViewModel
            {
                Courses = await this.coursesService.GetAll<CourseViewModel>(),
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
