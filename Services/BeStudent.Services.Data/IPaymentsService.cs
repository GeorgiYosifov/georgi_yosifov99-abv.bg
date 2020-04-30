namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface IPaymentsService
    {
        Task RegisterUserToSemesterAsync(string userId, int semesterId);

        Task RegisterUserToSubjectAsync(string userId, int subjectId);

        T GetUser<T>(string userId);

        T GetSemester<T>(string courseName, int nextNumber, int year);

        Semester GetSemester(string courseName, int nextNumber, int year);
    }
}
