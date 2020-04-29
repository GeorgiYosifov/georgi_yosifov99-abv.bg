namespace BeStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Course;
    using BeStudent.Web.ViewModels.Payment;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using PayPal.Core;
    using PayPal.v1.Payments;

    public class PaymentsController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IPaymentsService paymentsService;

        public PaymentsController(
            IConfiguration configuration,
            IPaymentsService paymentsService)
        {
            this.configuration = configuration;
            this.paymentsService = paymentsService;
        }

        [Authorize(Roles = "User, Lector")]
        public IActionResult ChooseCourse()
        {
            //this.dbContext.StudentSemesters.Add(new StudentSemester
            //{
            //    Student = user,
            //    Semester = semester,
            //});

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = this.paymentsService.GetUser<PaymentUserViewModel>(userId);
            var now = DateTime.Now;

            var viewModel = this.paymentsService
                .GetSemesters<PaymentCourseViewModel>(user.CourseName, user.HasPayment, now.Year);

            return this.View(viewModel);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Pay(decimal money)
        {
            var clientId = this.configuration.GetSection("PayPal").GetSection("clientId").Value;
            var secret = this.configuration.GetSection("PayPal").GetSection("secret").Value;

            var environment = new SandboxEnvironment(clientId, secret);
            var client = new PayPalHttpClient(environment);

            var portocol = this.HttpContext.Request.Scheme;
            var host = this.HttpContext.Request.Host;

            var returnUrl = $"{portocol}://{host}/Payments/Execute";
            var cancelUrl = $"{portocol}://{host}/Payments/Cancel";

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
                    var model = new PayViewModel()
                    {
                        Url = redirectUrl,
                    };

                    return this.View(model);
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
            var result = response.Result<Payment>();

            return this.Json($"gotovo {statusCode} {result.Payer.PayerInfo}");
        }
    }
}
