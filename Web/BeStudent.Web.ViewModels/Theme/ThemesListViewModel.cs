using System;
using System.Collections.Generic;
using System.Text;

namespace BeStudent.Web.ViewModels.Theme
{
    public class ThemesListViewModel
    {
        public string SubjectName { get; set; }

        public IEnumerable<ThemeViewModel> Themes { get; set; }
    }
}
