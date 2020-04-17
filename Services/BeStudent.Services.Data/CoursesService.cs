namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class CoursesService : ICoursesService
    {
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CoursesService(IDeletableEntityRepository<Course> courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public T ByName<T>(string name)
        {
            return this.courseRepository
                .All()
                .Where(c => c.Name == name)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task CreateAsync(string name)
        {
            var post = new Course()
            {
                Name = name,
            };

            await this.courseRepository.AddAsync(post);
            await this.courseRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.courseRepository.All().To<T>().ToList();
        }
    }
}
