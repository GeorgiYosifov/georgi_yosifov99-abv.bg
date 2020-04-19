namespace BeStudent.Web.ViewModels.Theme
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.File;
    using Ganss.XSS;

    public class ThemeViewModel : IMapFrom<Theme>
    {
        public string SubjectName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
