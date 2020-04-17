namespace BeStudent.Web.ViewModels.Subject
{
    using System.Collections.Generic;

    public class SubjectsListViewModel
    {
        public IEnumerable<SubjectForAllViewModel> Subjects { get; set; }
    }
}
