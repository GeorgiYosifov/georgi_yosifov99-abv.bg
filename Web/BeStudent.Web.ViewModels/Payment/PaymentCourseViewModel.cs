namespace BeStudent.Web.ViewModels.Payment
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentCourseViewModel : IMapFrom<Course>
    {
        public IEnumerable<PaymentSemesterViewModel> Semesters { get; set; }
    }
}
