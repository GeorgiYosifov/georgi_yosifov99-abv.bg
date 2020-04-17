namespace BeStudent.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;

    public class ExamsService : IExamsService
    {
        private readonly IDeletableEntityRepository<Subject> subjectRepository;
        private readonly IDeletableEntityRepository<Exam> examRepository;
        private readonly IDeletableEntityRepository<OnlineTest> onlineTestRepository;
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IDeletableEntityRepository<Answer> answerRepository;

        public ExamsService(
            IDeletableEntityRepository<Subject> subjectRepository,
            IDeletableEntityRepository<Exam> examRepository,
            IDeletableEntityRepository<OnlineTest> onlineTestRepository,
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Answer> answerRepository)
        {
            this.subjectRepository = subjectRepository;
            this.examRepository = examRepository;
            this.onlineTestRepository = onlineTestRepository;
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
        }

        public async Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, ExamType type)
        {
            var subject = this.subjectRepository.All().FirstOrDefault(s => s.Name == subjectName);

            var exam = new Exam
            {
                Type = type,
                Subject = subject,
                Title = title,
                Description = description,
            };

            exam.Files.Add(new File
            {
                CloudinaryFileUri = fileUri,
                FileDescription = fileDescription,
            });

            await this.examRepository.AddAsync(exam);
            await this.examRepository.SaveChangesAsync();
        }

        public async Task<int> CreateOnlineTestAsync(int examId, double minFor3, double range, double maxPoints, DateTime start, DateTime end, int duration)
        {
            var exam = this.examRepository.All().FirstOrDefault(e => e.Id == examId);

            var onlineTest = new OnlineTest
            {
                StartTime = start,
                EndTime = end,
                Duration = duration,
                Exam = exam,
                MinPointsFor3 = minFor3,
                Range = range,
                MaxPoints = maxPoints,
            };

            await this.onlineTestRepository.AddAsync(onlineTest);
            await this.onlineTestRepository.SaveChangesAsync();

            return onlineTest.Id;
        }

        public async Task<string> CreateQuestionAsync(int onlineTestId, string condition, string imageUri)
        {
            var onlineTest = this.onlineTestRepository.All().FirstOrDefault(t => t.Id == onlineTestId);

            var question = new Question
            {
                Condition = condition,
                File = new File
                {
                    CloudinaryFileUri = imageUri,
                    FileDescription = "Image",
                },
                OnlineTest = onlineTest,
            };

            await this.questionRepository.AddAsync(question);
            await this.questionRepository.SaveChangesAsync();

            return question.Id;
        }

        public async Task CreateQuestionWithAnswerAsync(int onlineTestId, string condition, string imageUri, AnswerType type, double points)
        {
            var onlineTest = this.onlineTestRepository.All().FirstOrDefault(t => t.Id == onlineTestId);

            var question = new Question
            {
                Condition = condition,
                File = new File
                {
                    CloudinaryFileUri = imageUri,
                    FileDescription = "Image",
                },
                OnlineTest = onlineTest,
            };

            question.Answers.Add(new Answer
            {
                Question = question,
                Points = points,
                Type = type,
            });

            await this.questionRepository.AddAsync(question);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task CreateAnswerAsync(string questionId, AnswerType type, string text, double points)
        {
            var question = this.questionRepository.All().FirstOrDefault(q => q.Id == questionId);

            var answer = new Answer
            {
                Text = text,
                Points = points,
                Type = type,
                Question = question,
            };

            await this.answerRepository.AddAsync(answer);
            await this.answerRepository.SaveChangesAsync();
        }
    }
}
