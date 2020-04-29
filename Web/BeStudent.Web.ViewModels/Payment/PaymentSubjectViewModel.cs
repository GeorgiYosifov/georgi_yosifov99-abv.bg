namespace BeStudent.Web.ViewModels.Payment
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentSubjectViewModel : IMapFrom<Subject>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
