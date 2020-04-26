namespace BeStudent.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Homework;
    using BeStudent.Web.ViewModels.SendFile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeworksController : BaseController
    {
        private readonly IThemesService themesService;
        private readonly IHomeworksService homeworksService;

        public HomeworksController(
            IThemesService themesService,
            IHomeworksService homeworksService)
        {
            this.themesService = themesService;
            this.homeworksService = homeworksService;
        }

        [Authorize(Roles = "User, Lector")]
        [HttpGet("Subjects/{subjectName}/Homeworks/{homeworkId}/Send")]
        public IActionResult Send(string subjectName, [FromQuery] DateTime deadline)
        {
            var now = DateTime.Now;
            if (deadline < now)
            {
                this.TempData["message"] = "The time to send file for this homework has expired!";
                return this.RedirectToAction("Themes", "Subjects", new { subjectName });
            }

            return this.View();
        }

        [Authorize(Roles = "User, Lector")]
        [HttpPost("Subjects/{subjectName}/Homeworks/{homeworkId}/Send")]
        public async Task<IActionResult> Send(string subjectName, int homeworkId, HomeworkSendInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var fileUri = string.Empty;
            if (input.File != null)
            {
                fileUri = this.themesService
                    .UploadFileToCloudinary(input.File.FileName, input.File.OpenReadStream());
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.homeworksService.SendAsync(userId, homeworkId, fileUri, input.FileDescription);

            return this.RedirectToAction("Themes", "Subjects", new { subjectName });
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Homeworks/{homeworkId}/Sended")]
        public IActionResult Sended(string subjectName, int homeworkId)
        {
            var viewModel = new SendFilesListViewModel
            {
                HomeworkId = homeworkId,
                SubjectName = subjectName,
                SendFiles = this.homeworksService.GetAllSendedFiles<SendFileViewModel>(homeworkId),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Homeworks/Create")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Subjects/{subjectName}/Homeworks/Create")]
        public async Task<IActionResult> Create(string subjectName, HomeworkCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var fileUri = string.Empty;
            if (input.File != null)
            {
                fileUri = this.themesService
                    .UploadFileToCloudinary(input.File.FileName, input.File.OpenReadStream());
                if (input.FileDescription == null)
                {
                    input.FileDescription = "document";
                }
            }

            await this.homeworksService.CreateAsync(subjectName, input.Title, input.Description, fileUri, input.FileDescription, input.Deadline);

            return this.RedirectToAction("Themes", "Subjects", new { subjectName });
        }
    }
}
