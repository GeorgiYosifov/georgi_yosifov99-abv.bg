namespace BeStudent.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentsService : IPaymentsService
    {
        private readonly IDeletableEntityRepository<Semester> semesterRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<StudentSemester> studentSemesterRepository;
        private readonly IDeletableEntityRepository<StudentSubject> studentSubjectRepository;

        public PaymentsService(
            IDeletableEntityRepository<Semester> semesterRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<StudentSemester> studentSemesterRepository,
            IDeletableEntityRepository<StudentSubject> studentSubjectRepository)
        {
            this.semesterRepository = semesterRepository;
            this.userRepository = userRepository;
            this.studentSemesterRepository = studentSemesterRepository;
            this.studentSubjectRepository = studentSubjectRepository;
        }

        public T GetUser<T>(string userId)
        {
            return this.userRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetSemester<T>(string courseName, int nextNumber, int year)
        {
            return this.semesterRepository
                .All()
                .Where(s => s.Year >= year && s.CourseName == courseName && s.Number == nextNumber)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task RegisterUserToSemesterAsync(string userId, int semesterId)
        {
            var userSemester = new StudentSemester
            {
                StudentId = userId,
                SemesterId = semesterId,
            };

            await this.studentSemesterRepository.AddAsync(userSemester);
            await this.studentSemesterRepository.SaveChangesAsync();

            var student = this.userRepository.All().FirstOrDefault(u => u.Id == userId);
            student.SemesterNumber++;
            await this.userRepository.SaveChangesAsync();
        }

        public async Task RegisterUserToSubjectAsync(string userId, int subjectId)
        {
            var userSubject = new StudentSubject
            {
                StudentId = userId,
                SubjectId = subjectId,
            };

            await this.studentSubjectRepository.AddAsync(userSubject);
            await this.studentSubjectRepository.SaveChangesAsync();
        }
    }
}
