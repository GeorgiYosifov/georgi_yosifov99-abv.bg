namespace BeStudent.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class PaymentsService : IPaymentsService
    {
        private readonly IDeletableEntityRepository<Semester> semesterRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<StudentSemester> studentSemesterRepository;
        private readonly IDeletableEntityRepository<StudentSubject> studentSubjectRepository;
        private readonly IRepository<PaymentAttempt> paymentAttemptRepository;
        private readonly IDeletableEntityRepository<Payment> paymentRepository;

        public PaymentsService(
            IDeletableEntityRepository<Semester> semesterRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<StudentSemester> studentSemesterRepository,
            IDeletableEntityRepository<StudentSubject> studentSubjectRepository,
            IRepository<PaymentAttempt> paymentAttemptRepository,
            IDeletableEntityRepository<Payment> paymentRepository)
        {
            this.semesterRepository = semesterRepository;
            this.userRepository = userRepository;
            this.studentSemesterRepository = studentSemesterRepository;
            this.studentSubjectRepository = studentSubjectRepository;
            this.paymentAttemptRepository = paymentAttemptRepository;
            this.paymentRepository = paymentRepository;
        }

        public async Task RegisterUserToSemesterAsync(string userId, int semesterId)
        {
            var userSemester = new StudentSemester
            {
                StudentId = userId,
                SemesterId = semesterId,
            };

            await this.studentSemesterRepository.AddAsync(userSemester);
            await this.studentSemesterRepository.SaveChangesAsync();

            var student = await this.userRepository.All().FirstOrDefaultAsync(u => u.Id == userId);
            student.SemesterNumber++;
            await this.userRepository.SaveChangesAsync();
        }

        public async Task RegisterUserToSubjectAsync(string userId, int subjectId)
        {
            var userSubject = new StudentSubject
            {
                StudentId = userId,
                SubjectId = subjectId,
            };

            await this.studentSubjectRepository.AddAsync(userSubject);
            await this.studentSubjectRepository.SaveChangesAsync();
        }

        public async Task<string> CreatePaymentAttemptAsync(string userId, int semesterId, decimal price)
        {
            var attempt = new PaymentAttempt
            {
                StudentId = userId,
                SemesterId = semesterId,
                Price = price,
            };

            await this.paymentAttemptRepository.AddAsync(attempt);
            await this.paymentAttemptRepository.SaveChangesAsync();

            return attempt.Id;
        }

        public async Task CreatePaymentAsync(string userId, int semesterId)
        {
            var payment = new Payment
            {
                StudentId = userId,
                SemesterId = semesterId,
            };

            await this.paymentRepository.AddAsync(payment);
            await this.paymentRepository.SaveChangesAsync();
        }

        public async Task<PaymentAttempt> GetPaymentAttempt(string id)
        {
            return await this.paymentAttemptRepository
                .All()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<T> GetUser<T>(string userId)
        {
            return await this.userRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetSemester<T>(int semesterId)
        {
            return await this.semesterRepository
                .All()
                .Where(s => s.Id == semesterId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetSemester<T>(string courseName, int nextNumber, int year)
        {
            return await this.semesterRepository
                .All()
                .Where(s => s.Year >= year && s.CourseName == courseName && s.Number == nextNumber)
                .To<T>()
                .FirstOrDefaultAsync();
        }
    }
}
