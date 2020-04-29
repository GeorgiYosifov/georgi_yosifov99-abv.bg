namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class SemestersService : ISemestersService
    {
        private readonly IDeletableEntityRepository<Semester> semesterRepository;

        public SemestersService(IDeletableEntityRepository<Semester> semesterRepository)
        {
            this.semesterRepository = semesterRepository;
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

        public IEnumerable<T> GetAll<T>(string courseName)
        {
            return this.semesterRepository
                .All()
                .Where(s => s.CourseName == courseName)
                .To<T>()
                .ToList();
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
