namespace BeStudent.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeStudent.Data.Models;

    public interface IExamsService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, ExamType type);

        Task<int> CreateOnlineTestAsync(int examId, double minFor3, double range, double maxPoints, DateTime start, DateTime end, int duration);

        Task<string> CreateQuestionAsync(int onlineTestId, string condition, string imageUri);

        Task CreateQuestionWithAnswerAsync(int onlineTestId, string condition, string imageUri, AnswerType type, double points);

        Task CreateAnswerAsync(string questionId, AnswerType type, string text, double points);
    }
}
