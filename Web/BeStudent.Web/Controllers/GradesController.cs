namespace BeStudent.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

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

        [Authorize(Roles = "Lector, User")]
        public async Task<IActionResult> All()
        {
            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await this.gradesService.GetStudent<StudentForGradesViewModel>(studentId);
            var lastStudentSemester = student.StudentSemesters.LastOrDefault();

            if (lastStudentSemester == null)
            {
                this.TempData["message"] = "You can pay your first semester now!";
                return this.RedirectToAction("ChooseCourse", "Payments");
            }

            var semesterId = lastStudentSemester.SemesterId;
            var viewModel = await this.gradesService.GetAll<SemesterForGradesViewModel>(semesterId);

            var studentSubjects = viewModel.Subjects
                .Where(s => s.StudentSubjects.FirstOrDefault(x => x.StudentId == studentId) != null)
                .ToList();

            viewModel.Subjects = studentSubjects;
            viewModel.StudentId = studentId;
            return this.View(viewModel);
        }
    }
}
