namespace BeStudent.Web.ViewModels.Theme
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class ThemeCreateInputModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile File { get; set; }

        public string FileDescription { get; set; }
    }
}