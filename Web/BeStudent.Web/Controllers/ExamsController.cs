namespace BeStudent.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BeStudent.Data.Models;
    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Exam;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ExamsController : BaseController
    {
        private readonly IExamsService examsService;
        private readonly IThemesService themesService;

        public ExamsController(
            IExamsService examsService,
            IThemesService themesService)
        {
            this.examsService = examsService;
            this.themesService = themesService;
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Exams/Create")]
        public IActionResult Create()
        {
            var model = new ExamCreateInputModel();

            return this.View(model);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Subjects/{subjectName}/Exams/Create")]
        public async Task<IActionResult> Create(string subjectName, ExamCreateInputModel input)
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

            await this.examsService.CreateAsync(subjectName, input.Title, input.Description, fileUri, input.FileDescription, input.ExamType);

            return this.RedirectToAction("Themes", "Subjects", new { subjectName });
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Exams/{examId}/CreateOnlineTest")]
        public IActionResult CreateOnlineTest(string subjectName)
        {
            var model = new OnlineTestCreateInputModel
            {
                SubjectName = subjectName,
            };

            return this.View(model);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Subjects/{subjectName}/Exams/{examId}/CreateOnlineTest")]
        public async Task<IActionResult> CreateOnlineTest(int examId, OnlineTestCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var onlineTestId = await this.examsService.CreateOnlineTestAsync(examId, input.MinPointsFor3, input.Range, input.MaxPoints, input.StartTime, input.EndTime, input.Duration);

            return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId });
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Exams/{onlineTestId}/CreateQuestion")]
        public IActionResult CreateQuestion(int onlineTestId)
        {
            var model = new QuestionAnswerViewModel
            {
                QuestionCreate = new QuestionCreateInputModel
                {
                    OnlineTestId = onlineTestId,
                },
                AnswerCreate = new AnswerCreateInputModel(),
            };

            return this.View(model);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Exams/{onlineTestId}/CreateQuestion")]
        public async Task<IActionResult> CreateQuestion(int onlineTestId, QuestionAnswerViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUri = string.Empty;
            if (input.QuestionCreate.Image != null)
            {
                imageUri = this.themesService
                    .UploadFileToCloudinary(input.QuestionCreate.Image.FileName, input.QuestionCreate.Image.OpenReadStream());
            }

            var answerType = input.AnswerCreate.Type.ToString("g");
            if (answerType == "RadioButtons")
            {
                var questionId = await this.examsService.CreateQuestionAsync(onlineTestId, input.QuestionCreate.Condition, imageUri);
                var numberOfAnswers = input.QuestionCreate.NumberOfAnswers;
                return this.RedirectToAction("CreateAnswers", "Exams", new { onlineTestId, questionId, numberOfAnswers });
            }
            else
            {
                await this.examsService.CreateQuestionWithAnswerAsync(onlineTestId, input.QuestionCreate.Condition, imageUri, input.AnswerCreate.Type, input.AnswerCreate.Points);
                return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId });
            }
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Exams/{onlineTestId}/CreateQuestion/{questionId}/CreateAnswers")]
        public IActionResult CreateAnswers(int onlineTestId, string questionId, [FromQuery] int numberOfAnswers)
        {
            var model = new AnswersListCreateInputModel
            {
                NumberOfAnswers = numberOfAnswers,
                OnlineTestId = onlineTestId,
                QuestionId = questionId,
                AnswerCreateInputs = new List<AnswerCreateInputModel>(),
            };

            return this.View(model);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Exams/{onlineTestId}/CreateQuestion/{questionId}/CreateAnswers")]
        public async Task<IActionResult> CreateAnswers(int onlineTestId, string questionId, AnswersListCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var answerType = AnswerType.RadioButtons;
            foreach (var answerInput in input.AnswerCreateInputs)
            {
                await this.examsService.CreateAnswerAsync(questionId, answerType, answerInput.Text, answerInput.Points);
            }

            return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId });
        }
    }
}
