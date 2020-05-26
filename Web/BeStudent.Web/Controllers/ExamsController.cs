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
    using BeStudent.Web.ViewModels.Grade;
    using BeStudent.Web.ViewModels.SendFile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ExamsController : BaseController
    {
        private readonly IExamsService examsService;
        private readonly IThemesService themesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExamsController(
            IExamsService examsService,
            IThemesService themesService,
            UserManager<ApplicationUser> userManager)
        {
            this.examsService = examsService;
            this.themesService = themesService;
            this.userManager = userManager;
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
                fileUri = await this.themesService
                    .UploadFileToCloudinary(input.File.FileName, input.File.OpenReadStream());
                if (input.FileDescription == null)
                {
                    input.FileDescription = "document";
                }
            }

            await this.examsService.CreateAsync(subjectName, input.Title, input.Description, fileUri, input.FileDescription, input.ExamType, input.OpenTime, input.CloseTime);

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
        public async Task<IActionResult> CreateQuestion(int onlineTestId, int currQuestion)
        {
            var numbers = await this.examsService.FindQuestionsCount(onlineTestId);
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
                imageUri = await this.themesService
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
            var onlineTestModel = await this.examsService.GetTest<OnlineTestStartViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (onlineTestModel.Students.FirstOrDefault(s => s.Id == studentId) != null)
            {
                return this.RedirectToAction("FinishTest", "Exams", new { onlineTestId });
            }

            var questionNumber = 0;
            var test = await this.examsService.GetTest<OnlineTestSolveViewModel>(onlineTestId);
            onlineTestModel.QuestionId = test.Questions[questionNumber].Id;
            onlineTestModel.QuestionNumber = questionNumber;
            return this.View(onlineTestModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/SolveQuestion/{questionId}/{questionNumber}")]
        public async Task<IActionResult> SolveQuestion(int onlineTestId, string questionId, int questionNumber, [FromQuery] long remainSeconds)
        {
            var questionModel = await this.examsService.GetQuestion<QuestionViewModel>(questionId);
            var onlineTestModel = await this.examsService.GetTest<OnlineTestStartViewModel>(onlineTestId);
            if (questionModel == null || onlineTestModel == null)
            {
                return this.NotFound();
            }

            var now = DateTime.Now;
            if (questionNumber == 0)
            {
                var endTimeOfTest = questionModel.OnlineTestEndTime;
                var remain = new TimeSpan();
                if (now.AddMinutes(questionModel.OnlineTestDuration) < endTimeOfTest)
                {
                    remain = TimeSpan.FromMinutes(questionModel.OnlineTestDuration - 1);
                }
                else
                {
                    remain = endTimeOfTest.Subtract(now);
                }

                remainSeconds = (long)remain.TotalSeconds;
            }

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

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.examsService.AddStudentInTest(onlineTestId, studentId);

            var secondsNow = now.Subtract(DateTime.MinValue).TotalSeconds;
            questionModel.SecondsBefore = (long)secondsNow;
            questionModel.RemainSeconds = remainSeconds;
            return this.View(questionModel);
        }

        [Authorize(Roles = "Lector, User")]
        [HttpPost("Exams/{onlineTestId}/SolveQuestion/{questionId}/{questionNumber}")]
        public async Task<IActionResult> SolveQuestion
            (int onlineTestId, string questionId, QuestionViewModel input, int questionNumber, [FromQuery] string type, int answerId, long remainSeconds, long secondsBefore)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            if (type == "RadioButtons")
            {
                var onlineTestModel = await this.examsService.GetQuestion<QuestionViewModel>(questionId);
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
            var count = await this.examsService.FindQuestionsCount(onlineTestId);
            if (questionNumber == count)
            {
                return this.RedirectToAction("FinishTest", "Exams", new { onlineTestId });
            }

            var test = await this.examsService.GetTest<OnlineTestSolveViewModel>(onlineTestId);
            questionId = test.Questions[questionNumber].Id;

            var secondsNow = DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
            remainSeconds = remainSeconds - ((long)secondsNow - secondsBefore);

            return this.RedirectToAction("SolveQuestion", "Exams", new { onlineTestId, questionId, questionNumber, remainSeconds });
        }

        [Authorize(Roles = "Lector, User")]
        [HttpGet("Exams/{onlineTestId}/FinishTest")]
        public async Task<IActionResult> FinishTest(int onlineTestId)
        {
            var onlineTestModel = await this.examsService.GetTest<OnlineTestFinishViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //DONT WORK
            //var isTestTask = this.examsService.IsTestWithClosedAnswers(onlineTestId);
            //var pointsTask = this.examsService.EarnedPoints(onlineTestId, studentId);

            //await Task.WhenAll(isTestTask, pointsTask);

            //var isTest = await isTestTask;
            //var points = await pointsTask;

            var isTest = await this.examsService.IsTestWithClosedAnswers(onlineTestId);
            var points = await this.examsService.EarnedPoints(onlineTestId, studentId);
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
        public async Task<IActionResult> ReviewTest(int onlineTestId, string studentId)
        {
            var onlineTestModel = await this.examsService.GetTest<OnlineTestReviewViewModel>(onlineTestId);
            if (onlineTestModel == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            onlineTestModel.HasOpenAnswers = false;
            if (user.Role == "Lector")
            {
                var count = onlineTestModel
                    .Questions
                    .Where(q => q.Answers.Any(a => a.Type.ToString() != "RadioButtons"))
                    .Count();
                if (count != 0)
                {
                    onlineTestModel.HasOpenAnswers = true;
                    onlineTestModel.SetPoints = new List<SetPointsInputModel>();
                    for (int i = 0; i < count; i++)
                    {
                        onlineTestModel.SetPoints.Add(new SetPointsInputModel());
                    }
                }
            }

            var studentToReview = onlineTestModel.Students.FirstOrDefault(s => s.Id == studentId);
            onlineTestModel.Student = studentToReview;
            return this.View(onlineTestModel);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Exams/{onlineTestId}/ReviewTest/{studentId}")]
        public async Task<IActionResult> ReviewTest(int onlineTestId, string studentId, OnlineTestReviewViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var points = input.SetPoints.Sum(f => f.Points);
            await this.examsService.CalculateGradeAsync(onlineTestId, studentId, points);
            return this.RedirectToAction("SendedTests", "Exams", new { onlineTestId });
        }

        [Authorize(Roles = "User")]
        [HttpGet("Subjects/{subjectName}/Exams/{examId}/SendSolution")]
        public async Task<IActionResult> SendSolution(string subjectName, int examId)
        {
            var examModel = await this.examsService.GetExam<ExamViewModel>(examId);

            var now = DateTime.Now;
            if (examModel.Open > now)
            {
                this.TempData["message"] = "You should wait until the exam became open!";
                return this.RedirectToAction("Themes", "Subjects", new { subjectName });
            }

            if (examModel.Close < now)
            {
                this.TempData["message"] = "The exam has already closed!";
                return this.RedirectToAction("Themes", "Subjects", new { subjectName });
            }

            var sendSolutionModel = new ExamSendSolutionInputModel
            {
                SubjectName = subjectName,
                ExamId = examId,
            };

            return this.View(sendSolutionModel);
        }

        [Authorize(Roles = "User")]
        [HttpPost("Subjects/{subjectName}/Exams/{examId}/SendSolution")]
        public async Task<IActionResult> SendSolution(string subjectName, int examId, ExamSendSolutionInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var fileUri = string.Empty;
            if (input.File != null)
            {
                fileUri = await this.themesService
                    .UploadFileToCloudinary(input.File.FileName, input.File.OpenReadStream());
            }

            var studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.examsService.SendSolutionAsync(studentId, examId, fileUri, input.FileDescription);

            return this.RedirectToAction("Themes", "Subjects", new { subjectName });
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Exams/{examId}/SendedSolutions")]
        public async Task<IActionResult> SendedSolutions(string subjectName, int examId)
        {
            var viewModel = new SendFilesListViewModel
            {
                ExamId = examId,
                SubjectName = subjectName,
                SendFiles = await this.examsService.GetAllSendedSolutions<SendFileViewModel>(examId),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Exams/{examId}/SetGrade")]
        public IActionResult SetGrade(string subjectName, int examId, [FromQuery] string studentId, int? sendFileId)
        {
            var gradeModel = new GradeSetInputModel
            {
                StudentId = studentId,
                ExamId = examId,
                SendFileId = sendFileId,
                SubjectName = subjectName,
            };

            return this.View(gradeModel);
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Subjects/{subjectName}/Exams/{examId}/SetGrade")]
        public async Task<IActionResult> SetGrade(string subjectName, int examId, string studentId, int? sendFileId, GradeSetInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.examsService.SetGradeAsync(input.Mark, input.Description, examId, studentId, sendFileId);

            if (sendFileId != null)
            {
                return this.RedirectToAction("SendedSolutions", "Exams", new { subjectName, examId });
            }
            else
            {
                return this.RedirectToAction("AllStudents", "Exams", new { subjectName, examId });
            }
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Exams/{onlineTestId}/SendedTests")]
        public async Task<IActionResult> SendedTests(int onlineTestId)
        {
            var viewModel = await this.examsService.GetTest<OnlineTestSendedTestsViewModel>(onlineTestId);

            return this.View(viewModel);
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Exams/{examId}/AllStudents")]
        public async Task<IActionResult> AllStudents(int examId, [FromQuery] string subjectName)
        {
            var viewModel = await this.examsService.GetExam<ExamForAllStudentsViewModel>(examId);
            viewModel.SubjectName = subjectName;

            return this.View(viewModel);
        }
    }
}
