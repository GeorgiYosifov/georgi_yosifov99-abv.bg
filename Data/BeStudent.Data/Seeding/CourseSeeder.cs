namespace BeStudent.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public class CourseSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Courses.Any())
            {
                return;
            }

            var courses = new List<string>() { "Design", "Computer Science", "Music", "Mathematics", "Business", "English", "History" };
            foreach (var course in courses)
            {
                await dbContext.Courses.AddAsync(new Course
                {
                    Name = course,
                });
            }
        }
    }
}
