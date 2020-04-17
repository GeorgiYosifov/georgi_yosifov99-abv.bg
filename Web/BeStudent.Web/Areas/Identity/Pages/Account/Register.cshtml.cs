namespace BeStudent.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using BeStudent.Data;
    using BeStudent.Data.Models;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext dbContext;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this._emailSender = emailSender;
            this.dbContext = dbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Course Name")]
            public string CourseName { get; set; }

            [Display(Name = "Semester Number")]
            public int? SemesterNumber { get; set; }

            [Display(Name = "Code for lector")]
            public string Code { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                };
                if (this.Input.CourseName != null && this.Input.SemesterNumber != null)
                {
                    user.CourseName = this.Input.CourseName;
                    user.Role = "User";
                    var semester = this.dbContext.Semesters
                        .FirstOrDefault(s => s.Course.Name == this.Input.CourseName && s.Number == this.Input.SemesterNumber);
                    this.dbContext.StudentSemesters.Add(new StudentSemester
                    {
                        Student = user,
                        Semester = semester,
                    });
                }
                else if (this.Input.Code != null)
                {
                    var code = this.dbContext.Codes
                        .FirstOrDefault(c => c.Id == this.Input.Code && c.SendTo == this.Input.Email);
                    var now = DateTime.UtcNow;
                    if (code != null)
                    {
                        if (code.IsUsed == true)
                        {
                            return this.BadRequest("The code had already used!");
                        }

                        if (code.ExpiresOn < now)
                        {
                            this.dbContext.Codes.Remove(code);
                            return this.BadRequest("The code had already expired!");
                        }

                        code.IsUsed = true;
                        user.Role = "Lector";
                    }
                    else
                    {
                        return this.BadRequest("Invalid code, try again!");
                    }
                }
                else
                {
                    return this.Page();
                }

                var result = await this._userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    if (user.Role == "Lector")
                    {
                        await this._userManager.AddToRoleAsync(user, "Lector");
                    }
                    else
                    {
                        await this._userManager.AddToRoleAsync(user, "User");
                    }

                    this._logger.LogInformation("User created a new account with password.");

                    var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: this.Request.Scheme);

                    await this._emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this._userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this._signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
