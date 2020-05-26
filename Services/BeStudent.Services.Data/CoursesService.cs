namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CoursesService : ICoursesService
    {
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CoursesService(IDeletableEntityRepository<Course> courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public async Task<T> ByName<T>(string name)
        {
            return await this.courseRepository
                .All()
                .Where(c => c.Name == name)
                .To<T>()
                .FirstOrDefaultAsync();
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

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.courseRepository
                .All()
                .To<T>()
                .ToListAsync();
        }
    }
}
