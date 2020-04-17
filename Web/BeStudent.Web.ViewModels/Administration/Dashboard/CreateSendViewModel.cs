namespace BeStudent.Web.ViewModels.Administration.Dashboard
{
    using System.ComponentModel.DataAnnotations;

    public class CreateSendViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Send code for lector on email")]
        public string Email { get; set; }
    }
}
