namespace BeStudent.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public class SemesterSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Semesters.Any())
            {
                return;
            }

            var semesters = new List<Semester>()
            {
                new Semester
                {
                    Number = 1,
                    Year = 2020,
                    CourseName = "Computer Science",
                    Course = dbContext.Courses.FirstOrDefault(c => c.Name == "Computer Science"),
                },
                new Semester
                {
                    Number = 2,
                    Year = 2020,
                    CourseName = "Computer Science",
                    Course = dbContext.Courses.FirstOrDefault(c => c.Name == "Computer Science"),
                },
                new Semester
                {
                    Number = 1,
                    Year = 2020,
                    CourseName = "History",
                    Course = dbContext.Courses.FirstOrDefault(c => c.Name == "History"),
                },
                new Semester
                {
                    Number = 2,
                    Year = 2020,
                    CourseName = "History",
                    Course = dbContext.Courses.FirstOrDefault(c => c.Name == "History"),
                },
            };
            foreach (var semester in semesters)
            {
                await dbContext.Semesters.AddAsync(semester);
            }
        }
    }
}
