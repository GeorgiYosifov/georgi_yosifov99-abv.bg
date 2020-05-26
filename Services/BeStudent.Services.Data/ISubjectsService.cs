namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISubjectsService
    {
        Task CreateAsync(int semesterId, string name, decimal price, string emails);

        Task<IEnumerable<T>> GetAll<T>(string userId);

        Task<T> GetThemesAsync<T>(string name);

        Task<T> FillCalendar<T>(string name);
    }
}
