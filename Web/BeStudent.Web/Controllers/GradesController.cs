namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Semester;
    using BeStudent.Web.ViewModels.Student;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class GradesController : BaseController
    {
        private readonly IGradesService gradesService;

        public GradesController(IGradesService gradesService)
        {
            this.gradesService = gradesService;
        }

        [Authorize(Roles = "User")]
        public IActionResult All()
        {
            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currStudentSemester = this.gradesService
                .GetStudent<StudentForGradesViewModel>(studentId)
                .StudentSemesters
                .LastOrDefault();

            if (currStudentSemester == null)
            {
                return this.Json("You dont have paid semester!");
            }

            var semesterId = currStudentSemester.SemesterId;
            var viewModel = this.gradesService.GetAll<SemesterForGradesViewModel>(semesterId);

            var studentSubjects = viewModel.Subjects
                .Where(s => s.StudentSubjects.FirstOrDefault(x => x.StudentId == studentId) != null)
                .ToList();

            viewModel.Subjects = studentSubjects;
            viewModel.StudentId = studentId;
            return this.View(viewModel);
        }
    }
}
