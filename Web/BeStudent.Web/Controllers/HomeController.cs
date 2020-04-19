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
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PayPal.Api;

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

        public ActionResult PaymentWithPaypal(string cancel = null)
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = this.Request.Query["PayerID"].ToString();
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = "https://localhost:44319/Home/PaymentWithPaypal?";

                    var guid = Guid.NewGuid().ToString();

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    this.HttpContext.Session.SetString(guid, createdPayment.id);

                    return this.Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = this.Request.Query["guid"].ToString();
                    var executedPayment = this.ExecutePayment(apiContext, payerId, this.HttpContext.Session.GetString(guid) as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return Json("FailureView inner");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("FailureView");
            }

            return Json("SuccessView");
        }

        private Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "5", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
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
