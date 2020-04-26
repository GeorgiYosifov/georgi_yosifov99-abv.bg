namespace BeStudent.Web.ViewModels.SendFile
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Grade;
    using BeStudent.Web.ViewModels.Student;

    public class SendFileViewModel : IMapFrom<SendFile>
    {
        public int Id { get; set; }

        public string CloudinaryFileUri { get; set; }

        public string FileDescription { get; set; }

        public StudentViewModel Student { get; set; }

        public GradeViewModel Grade { get; set; }
    }
}
