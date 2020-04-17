namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface ICoursesService
    {
        T ByName<T>(string name);

        IEnumerable<T> GetAll<T>();

        Task CreateAsync(string name);
    }
}
