namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class SubjectsService : ISubjectsService
    {
        private readonly IDeletableEntityRepository<Subject> subjectRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public SubjectsService(
            IDeletableEntityRepository<Subject> subjectRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.subjectRepository = subjectRepository;
            this.userRepository = userRepository;
        }

        public async Task CreateAsync(int semesterId, string name, string emails)
        {
            var subject = new Subject()
            {
                Name = name,
                SemesterId = semesterId,
            };

            var lectorsEmail = emails.Split().ToList();
            foreach (var lectorEmail in lectorsEmail)
            {
                subject.StudentSubjects.Add(new StudentSubject
                {
                    Student = this.userRepository.All().FirstOrDefault(l => l.Email == lectorEmail),
                    Subject = subject,
                });
            }

            await this.subjectRepository.AddAsync(subject);
            await this.subjectRepository.SaveChangesAsync();
        }

        public T FillCalendar<T>(string name)
        {
            return this.subjectRepository
                 .All()
                 .Where(s => s.Name == name)
                 .To<T>()
                 .FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>(string lectorId)
        {
            return this.subjectRepository
                .All()
                .Where(s => s.Id == s.StudentSubjects.FirstOrDefault(l => l.StudentId == lectorId).SubjectId)
                .To<T>()
                .ToList();
        }

        public T GetThemes<T>(string name)
        {
            return this.subjectRepository
                .All()
                .Where(s => s.Name == name)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
