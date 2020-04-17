namespace BeStudent.Web.ViewModels.Theme
{
    using System.Collections.Generic;

    using Ganss.XSS;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.File;

    public class ThemeViewModel : IMapFrom<Theme>
    {
        public string SubjectName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
