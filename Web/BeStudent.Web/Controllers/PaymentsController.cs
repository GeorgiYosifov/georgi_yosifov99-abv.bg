namespace BeStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Payment;
    using BeStudent.Web.ViewModels.Semester;
    using BeStudent.Web.ViewModels.Student;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using PayPal.Core;
    using PayPal.v1.Payments;

    public class PaymentsController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IPaymentsService paymentsService;
        private readonly IGradesService gradesService;

        public PaymentsController(
            IConfiguration configuration,
            IPaymentsService paymentsService,
            IGradesService gradesService)
        {
            this.configuration = configuration;
            this.paymentsService = paymentsService;
            this.gradesService = gradesService;
        }

        [Authorize(Roles = "User")]
        public IActionResult ChooseCourse()
        {
            var now = DateTime.Now;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = this.paymentsService.GetUser<PaymentUserViewModel>(userId);
            var nextSemester = 0;
            if (user.SemesterNumber == 0)
            {
                nextSemester = user.SemesterNumber + 1;
            }
            else
            {
                var semesterId = this.gradesService
                .GetStudent<StudentForGradesViewModel>(userId)
                .StudentSemesters
                .LastOrDefault()
                .SemesterId;

                var model = this.gradesService.GetAll<SemesterForGradesViewModel>(semesterId);

                var studentSubjects = model.Subjects
                    .Where(s => s.StudentSubjects.FirstOrDefault(x => x.StudentId == userId) != null)
                    .ToList();
                model.Subjects = studentSubjects;

                var examGrades = new List<double>();
                var homeworksGrades = new List<double>();
                foreach (var subject in model.Subjects)
                {
                    var innerGradeForExams = 0.0;
                    foreach (var exam in subject.Exams)
                    {
                        var grade = exam.Grades
                            .FirstOrDefault(g => g.StudentId == userId && g.ExamId == exam.Id);
                        if (grade == null)
                        {
                            this.TempData["message"] = "You must have grade on each exam!";
                            return this.RedirectToAction("Index", "Home");
                        }

                        innerGradeForExams += grade.Mark;
                    }

                    examGrades.Add(innerGradeForExams / subject.Exams.Count());

                    var innerGradeForHomeworks = 0.0;
                    foreach (var homework in subject.Homeworks)
                    {
                        var grade = homework.Grades
                            .FirstOrDefault(h => h.StudentId == userId && h.HomeworkId == homework.Id);
                        if (grade == null)
                        {
                            innerGradeForHomeworks += 2;
                        }
                        else
                        {
                            innerGradeForHomeworks += grade.Mark;
                        }
                    }

                    homeworksGrades.Add(innerGradeForHomeworks / subject.Homeworks.Count());
                }

                var gradeToPass = examGrades.Average();
                if (homeworksGrades.Average() >= 4)
                {
                    gradeToPass++;
                }

                if (gradeToPass < 3)
                {
                    this.TempData["message"] = "Your average grade is lower than 3! You should have more than 3 to pass this semester.";
                    return this.RedirectToAction("Index", "Home");
                }

                nextSemester = user.SemesterNumber + 1;
            }

            var viewModel = new PaymentCourseViewModel
            {
                Name = user.CourseName,
                Semester = this.paymentsService
                    .GetSemester<PaymentSemesterViewModel>(user.CourseName, nextSemester, now.Year),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Pay(decimal money)
        {
            money /= 100;
            var clientId = this.configuration.GetSection("PayPal").GetSection("clientId").Value;
            var secret = this.configuration.GetSection("PayPal").GetSection("secret").Value;

            var environment = new SandboxEnvironment(clientId, secret);
            var client = new PayPalHttpClient(environment);

            var portocol = this.HttpContext.Request.Scheme;
            var host = this.HttpContext.Request.Host;

            var returnUrl = $"{portocol}://{host}/Payments/Execute";
            var cancelUrl = $"{portocol}://{host}/Homes/Index";

            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = money.ToString(),
                            Currency = "USD",
                        },
                    },
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl,
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal",
                },
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            System.Net.HttpStatusCode statusCode;

            try
            {
                BraintreeHttp.HttpResponse response = await client.Execute(request);
                statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                string redirectUrl = null;
                foreach (LinkDescriptionObject link in result.Links)
                {
                    if (link.Rel.Equals("approval_url"))
                    {
                        redirectUrl = link.Href;
                    }
                }

                if (redirectUrl == null)
                {
                    return this.Json("Failed to find an approval_url in the response!");
                }
                else
                {
                    return this.Redirect(redirectUrl);
                }
            }
            catch (BraintreeHttp.HttpException ex)
            {
                statusCode = ex.StatusCode;
                var debugId = ex.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                return this.Json("Request failed!  HTTP response code was " + statusCode + ", debug ID was " + debugId);
            }
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Execute(string payerID, string paymentId)
        {
            var clientId = this.configuration.GetSection("PayPal").GetSection("clientId").Value;
            var secret = this.configuration.GetSection("PayPal").GetSection("secret").Value;

            var environment = new SandboxEnvironment(clientId, secret);
            var client = new PayPalHttpClient(environment);

            PaymentExecuteRequest request = new PaymentExecuteRequest(paymentId);
            request.RequestBody(new PaymentExecution()
            {
                PayerId = payerID,
            });

            var response = await client.Execute<PaymentExecuteRequest>(request);
            var statusCode = response.StatusCode;

            if (statusCode.ToString() == "OK")
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = this.paymentsService.GetUser<PaymentUserViewModel>(userId);
                var year = DateTime.Now.Year;

                var semester = this.paymentsService
                    .GetSemester<PaymentSemesterViewModel>(user.CourseName, user.SemesterNumber + 1, year);

                await this.paymentsService.RegisterUserToSemesterAsync(userId, semester.Id);
                foreach (var subject in semester.Subjects)
                {
                    await this.paymentsService.RegisterUserToSubjectAsync(userId, subject.Id);
                }

                this.TempData["message"] = $"Payment status code {statusCode}";
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                this.TempData["message"] = $"Payment status code {statusCode}";
                return this.RedirectToAction("Index", "Home");
            }
        }
    }
}
