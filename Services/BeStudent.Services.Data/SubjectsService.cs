namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class SubjectsService : ISubjectsService
    {
        private readonly IDeletableEntityRepository<Subject> subjectRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<StudentSemester> studentSemesterRepository;
        private readonly IDeletableEntityRepository<StudentSubject> studentSubjectRepository;
        private readonly IDeletableEntityRepository<Payment> paymentRepository;

        public SubjectsService(
            IDeletableEntityRepository<Subject> subjectRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<StudentSemester> studentSemesterRepository,
            IDeletableEntityRepository<StudentSubject> studentSubjectRepository,
            IDeletableEntityRepository<Payment> paymentRepository)
        {
            this.subjectRepository = subjectRepository;
            this.userRepository = userRepository;
            this.studentSemesterRepository = studentSemesterRepository;
            this.studentSubjectRepository = studentSubjectRepository;
            this.paymentRepository = paymentRepository;
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

            if (emails != null)
            {
                var lectorsEmail = emails.Split().ToList();
                foreach (var lectorEmail in lectorsEmail)
                {
                    var lector = this.userRepository
                        .All()
                        .FirstOrDefault(l => l.Email == lectorEmail && l.Role == "Lector");
                    if (lector == null)
                    {
                        continue;
                    }

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
        }

        public async Task<T> FillCalendar<T>(string name)
        {
            return await this.subjectRepository
                 .All()
                 .Where(s => s.Name == name)
                 .To<T>()
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>(string userId)
        {
            var user = await this.userRepository.All().FirstOrDefaultAsync(u => u.Id == userId);
            var subjects = this.subjectRepository.All();

            if (user.Role == "User")
            {
                var lastPayment = await this.paymentRepository
                    .All()
                    .Where(p => p.StudentId == userId)
                    .OrderByDescending(p => p.CreatedOn)
                    .FirstOrDefaultAsync();

                if (lastPayment == null)
                {
                    return new List<T>();
                }

                subjects = subjects
                    .Where(s => s.SemesterId == lastPayment.SemesterId);
            }
            else if (user.Role == "Lector")
            {
                subjects = subjects
                    .Where(s => s.Id == s.StudentSubjects.FirstOrDefault(u => u.StudentId == userId).SubjectId);
            }

            return await subjects.To<T>().ToListAsync();
        }

        public async Task<T> GetThemesAsync<T>(string name)
        {
            return await this.subjectRepository
                .AllAsNoTracking()
                .Where(s => s.Name == name)
                .To<T>()
                .FirstOrDefaultAsync();
        }
    }
}
