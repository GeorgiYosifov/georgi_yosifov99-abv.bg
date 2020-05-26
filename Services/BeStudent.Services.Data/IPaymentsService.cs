namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface IPaymentsService
    {
        Task RegisterUserToSemesterAsync(string userId, int semesterId);

        Task RegisterUserToSubjectAsync(string userId, int subjectId);

        Task<string> CreatePaymentAttemptAsync(string userId, int semesterId, decimal price);

        Task CreatePaymentAsync(string userId, int semesterId);

        Task<PaymentAttempt> GetPaymentAttempt(string id);

        Task<T> GetUser<T>(string userId);

        Task<T> GetSemester<T>(int semesterId);

        Task<T> GetSemester<T>(string courseName, int nextNumber, int year);
    }
}
