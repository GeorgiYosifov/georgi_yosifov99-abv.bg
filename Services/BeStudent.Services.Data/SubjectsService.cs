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
        private readonly IDeletableEntityRepository<StudentSemester> studentSemesterRepository;
        private readonly IDeletableEntityRepository<StudentSubject> studentSubjectRepository;

        public SubjectsService(
            IDeletableEntityRepository<Subject> subjectRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<StudentSemester> studentSemesterRepository,
            IDeletableEntityRepository<StudentSubject> studentSubjectRepository)
        {
            this.subjectRepository = subjectRepository;
            this.userRepository = userRepository;
            this.studentSemesterRepository = studentSemesterRepository;
            this.studentSubjectRepository = studentSubjectRepository;
        }

        public async Task CreateAsync(int semesterId, string name, decimal price, string emails)
        {
            var subject = new Subject()
            {
                Name = name,
                Price = price,
                SemesterId = semesterId,
            };
            await this.subjectRepository.AddAsync(subject);
            await this.subjectRepository.SaveChangesAsync();

            var lectorsEmail = emails.Split().ToList();
            foreach (var lectorEmail in lectorsEmail)
            {
                var lector = this.userRepository.All().FirstOrDefault(l => l.Email == lectorEmail);
                var lectorSubject = new StudentSubject
                {
                    Student = lector,
                    Subject = subject,
                };
                await this.studentSubjectRepository.AddAsync(lectorSubject);

                var lectorSemester = new StudentSemester
                {
                    Student = lector,
                    SemesterId = semesterId,
                };
                await this.studentSemesterRepository.AddAsync(lectorSemester);
            }

            await this.studentSubjectRepository.SaveChangesAsync();
            await this.studentSemesterRepository.SaveChangesAsync();
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
