using System;
using System.Collections.Generic;
using System.Text;

namespace BeStudent.Services.Data
{
    public interface IPaymentsService
    {
        T GetUser<T>(string userId);

        IEnumerable<T> GetSemesters<T>(string courseName, bool hasPayment, int year);
    }
}
