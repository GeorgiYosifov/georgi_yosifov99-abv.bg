namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    public interface ISemestersService
    {
        Task CreateAsync(int number, int year, string courseName, int courseId);

        Task<bool> AddLectorAsync(int subjectId, int semesterId, string email);

        Task<T> GetDetails<T>(int id);
    }
}
