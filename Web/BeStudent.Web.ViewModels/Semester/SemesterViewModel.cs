namespace BeStudent.Web.ViewModels.Semester
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class SemesterViewModel : IMapFrom<Semester>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int Year { get; set; }

        public SemesterType SemesterType
        {
            get
            {
                if (this.Number % 2 == 1)
                {
                    return SemesterType.Autumn;
                }
                else
                {
                    return SemesterType.Spring;
                }
            }
        }
    }
}
