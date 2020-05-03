namespace BeStudent.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class SemestersService : ISemestersService
    {
        private readonly IDeletableEntityRepository<Semester> semesterRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<StudentSubject> userSubjectRepository;
        private readonly IDeletableEntityRepository<StudentSemester> userSemesterRepository;

        public SemestersService(
            IDeletableEntityRepository<Semester> semesterRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<StudentSubject> userSubjectRepository,
            IDeletableEntityRepository<StudentSemester> userSemesterRepository)
        {
            this.semesterRepository = semesterRepository;
            this.userRepository = userRepository;
            this.userSubjectRepository = userSubjectRepository;
            this.userSemesterRepository = userSemesterRepository;
        }

        public async Task<bool> AddLectorAsync(int subjectId, int semesterId, string email)
        {
            var lector = this.userRepository
                .All()
                .FirstOrDefault(u => u.Email == email && u.Role == "Lector");

            if (lector == null)
            {
                return false;
            }

            var userSubject = new StudentSubject
            {
                StudentId = lector.Id,
                SubjectId = subjectId,
            };
            await this.userSubjectRepository.AddAsync(userSubject);
            await this.userSubjectRepository.SaveChangesAsync();

            var userSemester = new StudentSemester
            {
                StudentId = lector.Id,
                SemesterId = semesterId,
            };
            await this.userSemesterRepository.AddAsync(userSemester);
            await this.userSemesterRepository.SaveChangesAsync();

            return true;
        }

        public async Task CreateAsync(int number, int year, string courseName, int courseId)
        {
            var semester = new Semester
            {
                Number = number,
                Year = year,
                CourseName = courseName,
                CourseId = courseId,
            };

            await this.semesterRepository.AddAsync(semester);
            await this.semesterRepository.SaveChangesAsync();
        }

        public T GetDetails<T>(int id)
        {
            return this.semesterRepository
                .All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
