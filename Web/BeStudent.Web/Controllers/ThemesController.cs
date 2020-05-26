namespace BeStudent.Web.Controllers
{
    using System.Threading.Tasks;

    using BeStudent.Services.Data;
    using BeStudent.Web.ViewModels.Theme;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ThemesController : BaseController
    {
        private readonly IThemesService themesService;

        public ThemesController(IThemesService themesService)
        {
            this.themesService = themesService;
        }

        [Authorize(Roles = "Lector")]
        [HttpGet("Subjects/{subjectName}/Themes/Create")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = "Lector")]
        [HttpPost("Subjects/{subjectName}/Themes/Create")]
        public async Task<IActionResult> Create(string subjectName, ThemeCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var fileUri = string.Empty;
            if (input.File != null)
            {
                fileUri = await this.themesService
                    .UploadFileToCloudinary(input.File.FileName, input.File.OpenReadStream());
                if (input.FileDescription == null)
                {
                    input.FileDescription = "document";
                }
            }

            await this.themesService.CreateAsync(subjectName, input.Title, input.Description, fileUri, input.FileDescription);

            return this.RedirectToAction("Themes", "Subjects", new { subjectName });
        }
    }
}
