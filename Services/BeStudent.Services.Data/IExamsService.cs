namespace BeStudent.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface IExamsService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, ExamType type);

        Task<int> CreateOnlineTestAsync(int examId, double minFor3, double range, double maxPoints, DateTime start, DateTime end, int duration, int count);

        Task<string> CreateQuestionAsync(int onlineTestId, string condition, string imageUri);

        Task CreateQuestionWithAnswerAsync(int onlineTestId, string condition, string imageUri, AnswerType type);

        Task CreateAnswerAsync(string questionId, AnswerType type, string text, double points);

        int FindQuestionsCount(int onlineTestId);

        T GetQuestion<T>(string questionId);

        T GetTest<T>(int onlineTestId);
    }
}
