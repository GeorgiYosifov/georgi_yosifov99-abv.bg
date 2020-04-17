namespace BeStudent.Web.ViewModels.File
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class FileViewModel : IMapFrom<File>
    {
        public string CloudinaryFileUri { get; set; }

        public string FileDescription { get; set; }
    }
}
