namespace BeStudent.Web.ViewModels.File
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Student;

    public class SendFileViewModel : IMapFrom<SendFile>
    {
        public string CloudinaryFileUri { get; set; }

        public string FileDescription { get; set; }

        public StudentViewModel Student { get; set; }
    }
}
