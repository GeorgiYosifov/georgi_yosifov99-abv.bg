namespace BeStudent.Web.ViewModels.Payment
{
    using System.Collections.Generic;

    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class PaymentSemesterViewModel : IMapFrom<Semester>
    {
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

        public IEnumerable<Subject> Subjects { get; set; }
    }
}
