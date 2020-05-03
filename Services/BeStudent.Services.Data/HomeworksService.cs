namespace BeStudent.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class HomeworksService : IHomeworksService
    {
        private readonly IDeletableEntityRepository<Subject> subjectRepository;
        private readonly IDeletableEntityRepository<Homework> homeworkRepository;
        private readonly IRepository<SendFile> sendFileRepository;
        private readonly IRepository<File> fileRepository;
        private readonly IDeletableEntityRepository<Grade> gradeRepository;

        public HomeworksService(
            IDeletableEntityRepository<Subject> subjectRepository,
            IDeletableEntityRepository<Homework> homeworkRepository,
            IRepository<SendFile> sendFileRepository,
            IRepository<File> fileRepository,
            IDeletableEntityRepository<Grade> gradeRepository)
        {
            this.subjectRepository = subjectRepository;
            this.homeworkRepository = homeworkRepository;
            this.sendFileRepository = sendFileRepository;
            this.fileRepository = fileRepository;
            this.gradeRepository = gradeRepository;
        }

        public async Task AddNewFileAsync(int homeworkId, string fileUri, string fileDescription)
        {
            var file = new File
            {
                CloudinaryFileUri = fileUri,
                FileDescription = fileDescription,
                HomeworkId = homeworkId,
            };

            await this.fileRepository.AddAsync(file);
            await this.fileRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, DateTime deadline)
        {
            var subject = this.subjectRepository.All().FirstOrDefault(s => s.Name == subjectName);

            var homework = new Homework()
            {
                Title = title,
                Description = description,
                Subject = subject,
                Deadline = deadline,
            };

            await this.homeworkRepository.AddAsync(homework);
            await this.homeworkRepository.SaveChangesAsync();

            if (fileUri != string.Empty)
            {
                var file = new File()
                {
                    CloudinaryFileUri = fileUri,
                    FileDescription = fileDescription,
                    Homework = homework,
                };

                await this.fileRepository.AddAsync(file);
                await this.fileRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<T> GetAllSendedFiles<T>(int homeworkId)
        {
            return this.sendFileRepository
                .All()
                .Where(s => s.HomeworkId == homeworkId)
                .To<T>()
                .ToList();
        }

        public async Task SendAsync(string userId, int homeworkId, string fileUri, string fileDescription)
        {
            var sendFile = new SendFile
            {
                CloudinaryFileUri = fileUri,
                FileDescription = fileDescription,
                HomeworkId = homeworkId,
                StudentId = userId,
            };

            await this.sendFileRepository.AddAsync(sendFile);
            await this.sendFileRepository.SaveChangesAsync();
        }

        public async Task SetGradeAsync(double mark, string description, int homeworkId, string studentId, int sendFileId)
        {
            var grade = new Grade
            {
                Mark = mark,
                Description = description,
                HomeworkId = homeworkId,
                StudentId = studentId,
            };

            await this.gradeRepository.AddAsync(grade);
            await this.gradeRepository.SaveChangesAsync();

            this.sendFileRepository.All().FirstOrDefault(f => f.Id == sendFileId).Grade = grade;
            await this.sendFileRepository.SaveChangesAsync();
        }
    }
}
