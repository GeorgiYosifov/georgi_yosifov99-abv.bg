namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISemestersService
    {
        Task CreateAsync(int number, int year, string courseName, int courseId);

        Task AddLectorAsync(int subjectId, int semesterId, string email);

        IEnumerable<T> GetAll<T>(string courseName);

        T GetDetails<T>(int id);
    }
}
