namespace BeStudent.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentsService : IPaymentsService
    {
        private readonly IDeletableEntityRepository<Semester> semesterRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public PaymentsService(
            IDeletableEntityRepository<Semester> semesterRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.semesterRepository = semesterRepository;
            this.userRepository = userRepository;
        }

        public T GetUser<T>(string userId)
        {
            return this.userRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefault();
        }

        public IEnumerable<T> GetSemesters<T>(string courseName, bool hasPayment, int year)
        {
            return this.semesterRepository
                .All()
                .Where(s => hasPayment == true ? s.Number != 1 : s.Number == 1 && s.Year >= year && s.CourseName == courseName)
                .To<T>()
                .ToList();
        }
    }
}
