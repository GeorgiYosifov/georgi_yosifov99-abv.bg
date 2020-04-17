namespace BeStudent.Services.Data
{
    using System.Threading.Tasks;

    public interface IDashBoardService
    {
        Task SendCodeAsync(string email);
    }
}
