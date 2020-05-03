namespace BeStudent.Web.ViewModels.Subject
{
    using System.Collections.Generic;

    public class SubjectsListViewModel
    {
        public string Role { get; set; }

        public IEnumerable<SubjectForAllViewModel> Subjects { get; set; }
    }
}
