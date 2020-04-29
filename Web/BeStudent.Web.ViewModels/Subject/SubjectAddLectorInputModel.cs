namespace BeStudent.Web.ViewModels.Subject
{
    using System.ComponentModel.DataAnnotations;

    public class SubjectAddLectorInputModel
    {
        public int SemesterId { get; set; }

        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
