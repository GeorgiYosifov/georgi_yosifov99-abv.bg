namespace BeStudent.Services.Data
{
    public interface IGradesService
    {
        T GetStudent<T>(string studentId);

        T GetAll<T>(int semesterId);
    }
}
