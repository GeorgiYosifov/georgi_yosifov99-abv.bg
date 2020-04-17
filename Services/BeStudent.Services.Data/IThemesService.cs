namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IThemesService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription);

        string UploadFileToCloudinary(string name, Stream fileStream);
    }
}
