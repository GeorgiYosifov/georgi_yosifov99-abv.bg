namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface IPaymentsService
    {
        Task RegisterUserAsync(string userId, Semester semester);

        T GetUser<T>(string userId);

        T GetSemester<T>(string courseName, int nextNumber, int year);

        Semester GetSemester(string courseName, int nextNumber, int year);
    }
}
