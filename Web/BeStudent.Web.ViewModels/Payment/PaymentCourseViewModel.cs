namespace BeStudent.Web.ViewModels.Payment
{
    public class PaymentCourseViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string PaymentAttemptId { get; set; }

        public PaymentSemesterViewModel Semester { get; set; }
    }
}
