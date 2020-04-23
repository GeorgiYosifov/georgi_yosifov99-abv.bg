namespace BeStudent.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.Extensions.Configuration;

    public class ThemesService : IThemesService
    {
        private readonly IDeletableEntityRepository<Theme> themeRepository;
        private readonly IDeletableEntityRepository<Subject> subjectRepository;
        private readonly IConfiguration configuration;

        public ThemesService(
            IDeletableEntityRepository<Theme> themeRepository,
            IDeletableEntityRepository<Subject> subjectRepository,
            IConfiguration configuration)
        {
            this.themeRepository = themeRepository;
            this.subjectRepository = subjectRepository;
            this.configuration = configuration;
        }

        public async Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription)
        {
            var subject = this.subjectRepository.All().FirstOrDefault(s => s.Name == subjectName);

            var theme = new Theme()
            {
                Title = title,
                Description = description,
                Subject = subject,
            };

            if (fileUri != string.Empty)
            {
                theme.Files.Add(new BeStudent.Data.Models.File()
                {
                    CloudinaryFileUri = fileUri,
                    FileDescription = fileDescription,
                    Theme = theme,
                });
            }

            await this.themeRepository.AddAsync(theme);
            await this.themeRepository.SaveChangesAsync();
        }

        public string UploadFileToCloudinary(string name, Stream fileStream)
        {
            Account account = new Account
            {
                Cloud = this.configuration.GetSection("Cloudinary").GetSection("cloudName").Value,
                ApiKey = this.configuration.GetSection("Cloudinary").GetSection("apiKey").Value,
                ApiSecret = this.configuration.GetSection("Cloudinary").GetSection("apiSecret").Value,
            };

            Cloudinary cloudinary = new Cloudinary(account);

            var index = name.LastIndexOf('.');
            name = "file" + name.Substring(index);
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(name, fileStream),
            };

            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUri.AbsoluteUri;
        }
    }
}
