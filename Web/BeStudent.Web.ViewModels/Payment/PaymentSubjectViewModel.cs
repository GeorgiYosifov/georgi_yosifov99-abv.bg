namespace BeStudent.Web.ViewModels.Payment
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentSubjectViewModel : IMapFrom<Subject>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
