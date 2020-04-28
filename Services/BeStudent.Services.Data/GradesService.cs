namespace BeStudent.Services.Data
{
    using System.Linq;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class GradesService : IGradesService
    {
        private readonly IDeletableEntityRepository<Grade> gradeRepository;
        private readonly IDeletableEntityRepository<Semester> semesterRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> studentRepository;

        public GradesService(
            IDeletableEntityRepository<Grade> gradeRepository,
            IDeletableEntityRepository<Semester> semesterRepository,
            IDeletableEntityRepository<ApplicationUser> studentRepository)
        {
            this.gradeRepository = gradeRepository;
            this.semesterRepository = semesterRepository;
            this.studentRepository = studentRepository;
        }

        public T GetStudent<T>(string studentId)
        {
            return this.studentRepository
                .All()
                .Where(s => s.Id == studentId)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetAll<T>(int semesterId)
        {
            return this.semesterRepository
                .All()
                .Where(s => s.Id == semesterId)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
