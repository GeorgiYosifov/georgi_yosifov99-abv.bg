namespace BeStudent.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface IExamsService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, ExamType type, DateTime? open, DateTime? close);

        Task<int> CreateOnlineTestAsync(int examId, double minFor3, double range, double maxPoints, DateTime start, DateTime end, int duration, int count);

        Task<string> CreateQuestionAsync(int onlineTestId, string condition, string imageUri);

        Task CreateQuestionWithAnswerAsync(int onlineTestId, string condition, string imageUri, AnswerType type);

        Task CreateAnswerAsync(string questionId, AnswerType type, string text, double points);

        Task CreateDecisionAsync(string questionId, string studentId, int answerId, string content, string type);

        int FindQuestionsCount(int onlineTestId);

        T GetQuestion<T>(string questionId);

        T GetTest<T>(int onlineTestId);

        T GetExam<T>(int examId);

        Task AddStudentInTest(int onlineTestId, string studentId);

        Task<double> CalculateGradeAsync(int onlineTestId, string studentId, double points);

        IEnumerable<T> GetAllSendedSolutions<T>(int examId);

        Task SendSolutionAsync(string studentId, int examId, string fileUri, string fileDescription);

        Task SetGradeAsync(double mark, string description, int examId, string studentId, int sendFileId);
    }
}
