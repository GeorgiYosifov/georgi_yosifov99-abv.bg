namespace BeStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Parser;
    using BeStudent.Data.Models;
    using BeStudent.Services.Data;
    using BeStudent.Services.Messaging;
    using BeStudent.Web.ViewModels;
    using BeStudent.Web.ViewModels.Home;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using PayPal.Core;
    using PayPal.v1.Payments;

    public class HomeController : BaseController
    {
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IEmailSender sender;

        public HomeController(
            ICoursesService coursesService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSender sender)
        {
            this.coursesService = coursesService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.sender = sender;
        }

        public async Task<IActionResult> Test()
        {
            //var parser = new HtmlParser();
            //var webClient = new WebClient { Encoding = Encoding.UTF8 };

            //var url = "https://www.collegemagazine.com/";
            //var html = webClient.DownloadString(url);
            //var document = await parser.ParseDocumentAsync(html);
            //var links = new List<string>();
            //for (int i = 110744; i < 114000; i++)
            //{
            //    var link = document.QuerySelector($".post-{i} a.primary-cat")?.GetAttribute("href").ToString();
            //    if (link != null)
            //    {
            //        links.Add(link);
            //    }
            //}

            //var firstLink = links[0];
            //var text = "";
            //var innerHtml = webClient.DownloadString(firstLink);

            //return this.Ok(links);

            //return this.Ok(articleName);
            //var articleNames = new List<string>();

            //for (int i = 1; i < 600; i++)
            //{

            //}

            //await this.sender.SendEmailAsync("tapakt@abv.bg", "Goshe",
            //    "georgi_yosifov99@abv.bg", "Zdrasti", "<p>Sho staa</p>");
            //return this.Ok("Dobre e");

            //var result = await this.roleManager.CreateAsync(new ApplicationRole
            //{
            //    Name = "User",
            //});

            //var user = await this.userManager.GetUserAsync(this.User);
            //await this.userManager.AddToRoleAsync(user, "User");

            return this.Ok("He is there");
        }

        public async Task<IActionResult> Upload()
        {
            //Account account = new Account(
            //                "gogogrea",
            //                "173454941459328",
            //                "oe7bQk8t1FRBXNiFuJN4xdd_xfY");

            //Cloudinary cloudinary = new Cloudinary(account);

            //var uploadParams = new RawUploadParams()
            //{
            //    File = new FileDescription(@"C:\Users\Georgi\Desktop\reshenie-zad14.pdf"),
            //};
            //var uploadResult = cloudinary.Upload(uploadParams);

            //var uploadParams = new ImageUploadParams()
            //{
            //    File = new FileDescription(@"C:\Users\Georgi\Documents\Master.jpg"),
            //};
            //var uploadResult = cloudinary.Upload(uploadParams);

            return this.Ok("Tam sa");
        }

        public async Task<ActionResult> Pay()
        {
            var environment = new SandboxEnvironment("AewO0fN7c6GuMMO0fIQq_0qILiDOf4ASjy1cJTarVYmeNJPbePQ1mnV4g-HoTpvKMPShpl3q9x7hxILi", "EGXduWBuSMzgSI5G7ikWf1fBUn42tOCPNzXMh1Li6yumI4AIQatBZ0NOCYrtqLD5fbm2Y8B18I-aveD7");
            var client = new PayPalHttpClient(environment);

            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = "10",
                            Currency = "USD",
                        },
                    },
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl = "https://localhost:44319/Home/Execute",
                    CancelUrl = "https://localhost:44319/Home/Privacy",
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
                    // Didn't find an approval_url in response.Links
                    return this.Json("Failed to find an approval_url in the response!");
                }
                else
                {
                    var model = new PayModel()
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

        public async Task<IActionResult> Execute(string PayerID, string paymentId)
        {
            var environment = new SandboxEnvironment("AewO0fN7c6GuMMO0fIQq_0qILiDOf4ASjy1cJTarVYmeNJPbePQ1mnV4g-HoTpvKMPShpl3q9x7hxILi", "EGXduWBuSMzgSI5G7ikWf1fBUn42tOCPNzXMh1Li6yumI4AIQatBZ0NOCYrtqLD5fbm2Y8B18I-aveD7");
            var client = new PayPalHttpClient(environment);

            PaymentExecuteRequest request = new PaymentExecuteRequest(paymentId);
            request.RequestBody(new PaymentExecution()
            {
                PayerId = PayerID,
            });

            var response = await client.Execute<PaymentExecuteRequest>(request);
            var statusCode = response.StatusCode;
            var result = response.Result<Payment>();

            return this.Json($"gotovo {statusCode}");
        }

        //[Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
