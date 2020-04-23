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

            var onlineTestId = await this.examsService.CreateOnlineTestAsync(examId, input.MinPointsFor3, input.Range, input.MaxPoints, input.StartTime, input.EndTime, input.Duration, input.QuestionsCount);

            var currQuestion = 0;
            return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId, currQuestion });
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Exams/{onlineTestId}/CreateQuestion/{currQuestion}")]
        public IActionResult CreateQuestion(int onlineTestId, int currQuestion)
        {
            var numbers = this.examsService.FindQuestionsCount(onlineTestId);
            if (currQuestion == numbers)
            {
                return this.RedirectToAction("StartTest", "Exams", new { onlineTestId });
            }

            var model = new QuestionCreateInputModel
            {
                OnlineTestId = onlineTestId,
            };
            return this.View(model);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Exams/{onlineTestId}/CreateQuestion/{currQuestion}")]
        public async Task<IActionResult> CreateQuestion(int onlineTestId, int currQuestion, QuestionCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUri = string.Empty;
            if (input.Image != null)
            {
                imageUri = this.themesService
                    .UploadFileToCloudinary(input.Image.FileName, input.Image.OpenReadStream());
            }

            var answerType = input.Type.ToString("g");
            currQuestion++;

            if (answerType == "RadioButtons")
            {
                var questionId = await this.examsService.CreateQuestionAsync(onlineTestId, input.Condition, imageUri);
                var numberOfAnswers = input.NumberOfAnswers;
                return this.RedirectToAction("CreateAnswers", "Exams", new { onlineTestId, questionId, numberOfAnswers, currQuestion });
            }
            else
            {
                await this.examsService.CreateQuestionWithAnswerAsync(onlineTestId, input.Condition, imageUri, input.Type);
                return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId, currQuestion });
            }
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Exams/{onlineTestId}/CreateQuestion/{questionId}/CreateAnswers/{currQuestion}")]
        public IActionResult CreateAnswers(int onlineTestId, string questionId, [FromQuery] int numberOfAnswers)
        {
            var model = new AnswersListCreateInputModel
            {
                OnlineTestId = onlineTestId,
                QuestionId = questionId,
                AnswerCreateInputs = new List<AnswerCreateInputModel>(),
            };
            for (int i = 0; i < numberOfAnswers; i++)
            {
                model.AnswerCreateInputs.Add(new AnswerCreateInputModel());
            }

            return this.View(model);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Exams/{onlineTestId}/CreateQuestion/{questionId}/CreateAnswers/{currQuestion}")]
        public async Task<IActionResult> CreateAnswers
            (int onlineTestId, string questionId, int currQuestion, AnswersListCreateInputModel input)
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

            return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId, currQuestion });
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/StartTest")]
        public IActionResult StartTest(int onlineTestId)
        {
            var onlineTestModel = this.examsService.GetTest<OnlineTestViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var questionNumber = 0;
            var test = this.examsService.GetTest<OnlineTestSolveViewModel>(onlineTestId);
            onlineTestModel.QuestionId = test.Questions[questionNumber].Id;

            return this.View(onlineTestModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/SolveTest/{questionId}")]
        public IActionResult SolveTest(int onlineTestId, string questionId, [FromQuery] int questionNumber)
        {
            var onlineTestModel = this.examsService.GetQuestion<QuestionViewModel>(questionId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            return this.View(onlineTestModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpPost("Exams/{onlineTestId}/SolveTest/{questionId}")]
        public async Task<IActionResult> SolveTest
            (int onlineTestId, string questionId, [FromQuery] int questionNumber, QuestionViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var test = this.examsService.GetTest<OnlineTestSolveViewModel>(onlineTestId);
            onlineTestModel.QuestionId = test.Questions[questionNumber].Id;

            return this.RedirectToAction("CreateQuestion", "Exams", new { onlineTestId, currQuestion });
        }
    }
}
