namespace BeStudent.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public class SubjectSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Subjects.Any())
            {
                return;
            }

            var subjectsForIT = new List<string>() { "Math", "English", "Sport" };
            var subjectsForHistory = new List<string>() { "History of the future", "Ancient", "Sport" };
            var subjectsForMusic = new List<string>() { "English", "Ideas in business", "Made of the song" };
            var subjectsForBusiness = new List<string>() { "English", "Ideas in business", "Economics in the future" };

            foreach (var subject in subjectsForIT)
            {
                await dbContext.Subjects.AddAsync(new Subject
                {
                    Name = subject,
                    Semester = dbContext.Semesters.FirstOrDefault(s => s.Course.Name == "Computer Science" && s.Number == 2),
                });
            }

            foreach (var subject in subjectsForHistory)
            {
                await dbContext.Subjects.AddAsync(new Subject
                {
                    Name = subject,
                    Semester = dbContext.Semesters.FirstOrDefault(s => s.Course.Name == "History" && s.Number == 1),
                });
            }

            //foreach (var subject in subjectsForMusic)
            //{
            //    await dbContext.Subjects.AddAsync(new Subject
            //    {
            //        Name = subject,
            //        Semester = dbContext.Semesters.FirstOrDefault(s => s.Course.Name == "Music"),
            //    });
            //}
        }
    }
}
