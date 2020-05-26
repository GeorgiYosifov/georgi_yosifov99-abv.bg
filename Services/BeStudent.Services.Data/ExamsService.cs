namespace BeStudent.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;
    using BeStudent.Web.ViewModels.Exam;
    using Microsoft.EntityFrameworkCore;

    public class ExamsService : IExamsService
    {
        private readonly IDeletableEntityRepository<Subject> subjectRepository;
        private readonly IDeletableEntityRepository<Exam> examRepository;
        private readonly IDeletableEntityRepository<OnlineTest> onlineTestRepository;
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IDeletableEntityRepository<Answer> answerRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> studentRepository;
        private readonly IDeletableEntityRepository<Decision> decisionRepository;
        private readonly IDeletableEntityRepository<Grade> gradeRepository;
        private readonly IRepository<SendFile> sendFileRepository;

        public ExamsService(
            IDeletableEntityRepository<Subject> subjectRepository,
            IDeletableEntityRepository<Exam> examRepository,
            IDeletableEntityRepository<OnlineTest> onlineTestRepository,
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Answer> answerRepository,
            IDeletableEntityRepository<ApplicationUser> studentRepository,
            IDeletableEntityRepository<Decision> decisionRepository,
            IDeletableEntityRepository<Grade> gradeRepository,
            IRepository<SendFile> sendFileRepository)
        {
            this.subjectRepository = subjectRepository;
            this.examRepository = examRepository;
            this.onlineTestRepository = onlineTestRepository;
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.studentRepository = studentRepository;
            this.decisionRepository = decisionRepository;
            this.gradeRepository = gradeRepository;
            this.sendFileRepository = sendFileRepository;
        }

        public async Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, ExamType type, DateTime? open, DateTime? close)
        {
            var subject = await this.subjectRepository.All().FirstOrDefaultAsync(s => s.Name == subjectName);

            var exam = new Exam
            {
                Type = type,
                Subject = subject,
                Title = title,
                Description = description,
                Open = open,
                Close = close,
            };

            if (fileUri != string.Empty)
            {
                exam.Files.Add(new File
                {
                    CloudinaryFileUri = fileUri,
                    FileDescription = fileDescription,
                });
            }

            if (type.ToString() == "PresentExam")
            {
                exam.Students = this.studentRepository
                    .All()
                    .Where(s => s.StudentSubjects.FirstOrDefault(x => x.Subject.Name == subjectName) != null)
                    .ToHashSet<ApplicationUser>();
            }

            await this.examRepository.AddAsync(exam);
            await this.examRepository.SaveChangesAsync();
        }

        public async Task<int> CreateOnlineTestAsync(int examId, double minFor3, double range, double maxPoints, DateTime start, DateTime end, int duration, int count)
        {
            var onlineTest = new OnlineTest
            {
                QuestionsCount = count,
                StartTime = start,
                EndTime = end,
                Duration = duration,
                ExamId = examId,
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
            var question = new Question
            {
                Condition = condition,
                OnlineTestId = onlineTestId,
            };

            if (imageUri != string.Empty)
            {
                question.File = new File
                {
                    CloudinaryFileUri = imageUri,
                    FileDescription = "Image",
                };
            }

            await this.questionRepository.AddAsync(question);
            await this.questionRepository.SaveChangesAsync();

            return question.Id;
        }

        public async Task CreateQuestionWithAnswerAsync(int onlineTestId, string condition, string imageUri, AnswerType type)
        {
            var question = new Question
            {
                Condition = condition,
                OnlineTestId = onlineTestId,
            };

            if (imageUri != string.Empty)
            {
                question.File = new File
                {
                    CloudinaryFileUri = imageUri,
                    FileDescription = "Image",
                };
            }

            question.Answers.Add(new Answer
            {
                Question = question,
                Type = type,
            });

            await this.questionRepository.AddAsync(question);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task CreateAnswerAsync(string questionId, AnswerType type, string text, double points)
        {
            var answer = new Answer
            {
                Text = text,
                Points = points,
                Type = type,
                QuestionId = questionId,
            };

            await this.answerRepository.AddAsync(answer);
            await this.answerRepository.SaveChangesAsync();
        }

        public async Task<int> FindQuestionsCount(int onlineTestId)
        {
            var onlineTest = await this.onlineTestRepository
                .All()
                .FirstOrDefaultAsync(t => t.Id == onlineTestId);

            return onlineTest.QuestionsCount;
        }

        public async Task<T> GetTest<T>(int onlineTestId)
        {
            return await this.onlineTestRepository
                .All()
                .Where(s => s.Id == onlineTestId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetQuestion<T>(string questionId)
        {
            return await this.questionRepository
                .All()
                .Where(q => q.Id == questionId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task CreateDecisionAsync(string questionId, string studentId, int answerId, string content, string type)
        {
            var answer = await this.answerRepository.All().FirstOrDefaultAsync(a => a.Id == answerId);
            var points = 0.0;
            if (answer != null)
            {
                points = answer.Points ?? 0;
            }

            var decision = new Decision
            {
                Content = content,
                Points = points,
                Type = type,
                QuestionId = questionId,
                StudentId = studentId,
            };

            await this.decisionRepository.AddAsync(decision);
            await this.decisionRepository.SaveChangesAsync();
        }

        public async Task AddStudentInTest(int onlineTestId, string studentId)
        {
            var test = await this.onlineTestRepository.All().FirstOrDefaultAsync(t => t.Id == onlineTestId);
            var student = await this.studentRepository.All().FirstOrDefaultAsync(s => s.Id == studentId);
            test.Students.Add(student);

            await this.onlineTestRepository.SaveChangesAsync();
        }

        public async Task<double> CalculateGradeAsync(int onlineTestId, string studentId, double points)
        {
            var test = await this.onlineTestRepository.All().FirstOrDefaultAsync(t => t.Id == onlineTestId);
            var examId = test.ExamId;

            var mark = 0.0;
            if (points < test.MinPointsFor3)
            {
                mark = 2.0;
            }

            var diff = points - test.MinPointsFor3;
            var digit = diff / test.Range;
            if (mark == 0.0)
            {
                mark += digit + 3.0;
                if (points == test.MaxPoints)
                {
                    mark = 6.0;
                }
            }

            var grade = new Grade
            {
                Mark = mark,
                StudentId = studentId,
                ExamId = examId,
            };

            await this.gradeRepository.AddAsync(grade);
            await this.gradeRepository.SaveChangesAsync();
            return mark;
        }

        public async Task<IEnumerable<T>> GetAllSendedSolutions<T>(int examId)
        {
            return await this.sendFileRepository
                .All()
                .Where(s => s.ExamId == examId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetExam<T>(int examId)
        {
            return await this.examRepository
                .All()
                .Where(e => e.Id == examId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task SendSolutionAsync(string studentId, int examId, string fileUri, string fileDescription)
        {
            var sendFile = new SendFile
            {
                CloudinaryFileUri = fileUri,
                FileDescription = fileDescription,
                ExamId = examId,
                StudentId = studentId,
            };

            await this.sendFileRepository.AddAsync(sendFile);
            await this.sendFileRepository.SaveChangesAsync();
        }

        public async Task SetGradeAsync(double mark, string description, int examId, string studentId, int? sendFileId)
        {
            var grade = new Grade
            {
                Mark = mark,
                Description = description,
                ExamId = examId,
                StudentId = studentId,
            };

            await this.gradeRepository.AddAsync(grade);
            await this.gradeRepository.SaveChangesAsync();

            if (sendFileId != null)
            {
                this.sendFileRepository.All().FirstOrDefaultAsync(f => f.Id == sendFileId).GetAwaiter().GetResult().Grade = grade;
                await this.sendFileRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> IsTestWithClosedAnswers(int onlineTestId)
        {
            var test = await this.onlineTestRepository
                .AllAsNoTracking()
                .Where(t => t.Id == onlineTestId)
                .To<OnlineTestSolveViewModel>()
                .FirstOrDefaultAsync();

            foreach (var question in test.Questions)
            {
                var answerType = question.Answers.FirstOrDefault().Type.ToString();
                if (answerType == "InputFieldUp20Chars" || answerType == "InputFieldTiny")
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<double> EarnedPoints(int onlineTestId, string studentId)
        {
            var test = await this.onlineTestRepository
               .AllAsNoTracking()
               .Where(t => t.Id == onlineTestId)
               .To<OnlineTestSolveViewModel>()
               .FirstOrDefaultAsync();

            var points = 0.0;
            foreach (var question in test.Questions)
            {
                var decision = question.Decisions.FirstOrDefault(d => d.StudentId == studentId);
                if (decision != null)
                {
                    points += decision.Points;
                }
            }

            return points;
        }
    }
}
