namespace BeStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
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
        public async Task<IActionResult> StartTest(int onlineTestId)
        {
            var onlineTestModel = this.examsService.GetTest<OnlineTestStartViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (onlineTestModel.Students.FirstOrDefault(s => s.Id == studentId) != null)
            //{
            //    return this.RedirectToAction("FinishTest", "Exams", new { onlineTestId });
            //}

            await this.examsService.AddStudentInTest(onlineTestId, studentId);

            var questionNumber = 0;
            var test = this.examsService.GetTest<OnlineTestSolveViewModel>(onlineTestId);
            onlineTestModel.QuestionId = test.Questions[questionNumber].Id;
            onlineTestModel.QuestionNumber = questionNumber;
            return this.View(onlineTestModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/SolveQuestion/{questionId}/{questionNumber}")]
        public IActionResult SolveQuestion(int onlineTestId, string questionId, int questionNumber)
        {
            var questionModel = this.examsService.GetQuestion<QuestionViewModel>(questionId);
            var onlineTestModel = this.examsService.GetTest<OnlineTestStartViewModel>(onlineTestId);
            if (questionModel == null || onlineTestModel == null)
            {
                return this.NotFound();
            }

            var now = DateTime.Now;
            var theDate = questionModel.OnlineTestEndTime;
            var remain = theDate.Subtract(now);
            var remainSeconds = (int)remain.TotalMinutes * 60;

            if (onlineTestModel.StartTime > now)
            {
                this.TempData["message"] = "You should wait until the test became open!";
                return this.RedirectToAction("StartTest", "Exams", new { onlineTestId });
            }

            if (onlineTestModel.EndTime < now)
            {
                this.TempData["message"] = "The test has already closed!";
                return this.RedirectToAction("StartTest", "Exams", new { onlineTestId });
            }

            questionModel.RemainSeconds = remainSeconds;
            return this.View(questionModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpPost("Exams/{onlineTestId}/SolveQuestion/{questionId}/{questionNumber}")]
        public async Task<IActionResult> SolveQuestion
            (int onlineTestId, string questionId, QuestionViewModel input, int questionNumber, [FromQuery] string type, int answerId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            if (type == "RadioButtons")
            {
                var onlineTestModel = this.examsService.GetQuestion<QuestionViewModel>(questionId);
                var answer = onlineTestModel.Answers.FirstOrDefault(a => a.Text == input.Value);
                if (answer != null)
                {
                    answerId = answer.Id;
                }
                else
                {
                    answerId = 0;
                }
            }

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.examsService.CreateDecisionAsync(questionId, studentId, answerId, input.Value, type);

            questionNumber++;
            var count = this.examsService.FindQuestionsCount(onlineTestId);
            if (questionNumber == count)
            {
                return this.RedirectToAction("FinishTest", "Exams", new { onlineTestId });
            }

            var test = this.examsService.GetTest<OnlineTestSolveViewModel>(onlineTestId);
            questionId = test.Questions[questionNumber].Id;

            return this.RedirectToAction("SolveQuestion", "Exams", new { onlineTestId, questionId, questionNumber });
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/FinishTest")]
        public async Task<IActionResult> FinishTest(int onlineTestId)
        {
            var onlineTestModel = this.examsService.GetTest<OnlineTestFinishViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var isTest = true;
            foreach (var question in onlineTestModel.Questions)
            {
                var answerType = question.Answers.FirstOrDefault().Type.ToString();
                if (answerType == "InputFieldUp20Chars" || answerType == "InputFieldTiny")
                {
                    isTest = false;
                    break;
                }
            }

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var points = 0.0;
            foreach (var question in onlineTestModel.Questions)
            {
                var decision = question.Decisions.FirstOrDefault(d => d.StudentId == studentId);
                if (decision != null)
                {
                    points += decision.Points;
                }
            }

            if (isTest == true)
            {
                onlineTestModel.Mark = await this.examsService.CalculateGradeAsync(onlineTestId, studentId, points);
            }

            onlineTestModel.Points = points;
            onlineTestModel.StudentId = studentId;
            return this.View(onlineTestModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/ReviewTest/{studentId}")]
        public IActionResult ReviewTest(int onlineTestId, string studentId)
        {
            var onlineTestModel = this.examsService.GetTest<OnlineTestReviewViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var student = onlineTestModel.Students.FirstOrDefault(s => s.Id == studentId);
            onlineTestModel.Student = student;
            return this.View(onlineTestModel);
        }
    }
}
