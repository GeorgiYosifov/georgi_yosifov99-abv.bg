namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICoursesService
    {
        Task<T> ByName<T>(string name);

        Task<IEnumerable<T>> GetAll<T>();

        Task CreateAsync(string name);
    }
}
