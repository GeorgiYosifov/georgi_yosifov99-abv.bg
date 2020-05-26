namespace BeStudent.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class GradesService : IGradesService
    {
        private readonly IDeletableEntityRepository<Semester> semesterRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> studentRepository;

        public GradesService(
            IDeletableEntityRepository<Semester> semesterRepository,
            IDeletableEntityRepository<ApplicationUser> studentRepository)
        {
            this.semesterRepository = semesterRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<T> GetStudent<T>(string studentId)
        {
            return await this.studentRepository
                .All()
                .Where(s => s.Id == studentId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetAll<T>(int semesterId)
        {
            return await this.semesterRepository
                .All()
                .Where(s => s.Id == semesterId)
                .To<T>()
                .FirstOrDefaultAsync();
        }
    }
}
