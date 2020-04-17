namespace BeStudent.Services.Data
{
    using System.Collections.Generic;

    public interface ISemestersService
    {
        IEnumerable<T> GetAll<T>(string courseName);

        T GetDetails<T>(int id);
    }
}
