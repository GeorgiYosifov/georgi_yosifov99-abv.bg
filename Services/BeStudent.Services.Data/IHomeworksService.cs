namespace BeStudent.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHomeworksService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, DateTime deadline);

        Task SendAsync(string userId, int homeworkId, string fileUri, string fileDescription);

        Task SetGradeAsync(double mark, string description, int homeworkId, string studentId, int sendFileId);

        Task AddNewFileAsync(int homeworkId, string fileUri, string fileDescription);

        Task<IEnumerable<T>> GetAllSendedFiles<T>(int homeworkId);
    }
}
