namespace BeStudent.Web.ViewModels.Subject
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class SubjectViewModel : IMapFrom<Subject>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int SemesterId { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Lectors { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Subject, SubjectViewModel>()
                .ForMember(x => x.Lectors, options =>
                {
                    options.MapFrom(s => s.StudentSubjects
                                .Where(l => l.Student.Role == "Lector")
                                .Select(l => l.Student.FirstName + " " + l.Student.LastName));
                });
        }
    }
}
