namespace BeStudent.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IThemesService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription);

        Task<string> UploadFileToCloudinary(string name, Stream fileStream);
    }
}
