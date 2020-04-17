namespace BeStudent.Web.ViewModels.Student
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class StudentViewModel : IHaveCustomMappings, IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public IEnumerable<string> Subjects { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, StudentViewModel>()
                .ForMember(x => x.Subjects, options =>
                {
                    options.MapFrom(s => s.StudentSubjects.Select(s => s.Subject.Name));
                });
        }
    }
}
