using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BeStudent.Services.Data
{
    public interface IHomeworksService
    {
        Task CreateAsync(string subjectName, string title, string description, string fileUri, string fileDescription, DateTime deadline);

        Task SendAsync(string userId, int homeworkId, string fileUri, string fileDescription);

        IEnumerable<T> GetAllSendedFiles<T>(int homeworkId);
    }
}
