namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISubjectsService
    {
        Task CreateAsync(int semesterId, string name, string emails);

        IEnumerable<T> GetAll<T>(string lectorId);

        T GetThemes<T>(string name);

        T FillCalendar<T>(string name);
    }
}
