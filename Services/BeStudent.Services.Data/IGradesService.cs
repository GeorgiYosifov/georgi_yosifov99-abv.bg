namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    public interface IGradesService
    {
        Task<T> GetStudent<T>(string studentId);

        Task<T> GetAll<T>(int semesterId);
    }
}
