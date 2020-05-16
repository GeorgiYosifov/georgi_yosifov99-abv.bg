// ReSharper disable VirtualMemberCallInConstructor
namespace BeStudent.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeStudent.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.StudentSubjects = new HashSet<StudentSubject>();
            this.StudentSemesters = new HashSet<StudentSemester>();
            this.Payments = new HashSet<Payment>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CodeForRegister { get; set; }

        public string Description { get; set; }

        public string CourseName { get; set; }

        public int? SemesterNumber { get; set; }

        public string Role { get; set; }

        public virtual ICollection<StudentSemester> StudentSemesters { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
