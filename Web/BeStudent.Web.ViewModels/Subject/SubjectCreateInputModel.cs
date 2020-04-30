namespace BeStudent.Web.ViewModels.Subject
{
    using System.ComponentModel.DataAnnotations;

    public class SubjectCreateInputModel
    {
        public int SemesterId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public string Emails { get; set; }
    }
}
