namespace BeStudent.Web.ViewModels.Subject
{
    using System.ComponentModel.DataAnnotations;

    public class SubjectCreateInputModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public string Emails { get; set; }
    }
}
