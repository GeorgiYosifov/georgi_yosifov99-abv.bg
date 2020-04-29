namespace BeStudent.Web.ViewModels.Payment
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentUserViewModel : IMapFrom<ApplicationUser>
    {
        public string CourseName { get; set; }

        public int SemesterNumber { get; set; }
    }
}
