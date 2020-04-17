namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

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
