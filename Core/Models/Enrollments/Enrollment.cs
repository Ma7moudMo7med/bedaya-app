using Core.Enums;
using Core.Models.Courses;
using Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Enrollments
{
    public class Enrollment
    {
        public Enrollment()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public StudentStatus Status { get; set; }   // active, completed, cancelled
        public DateTime? CompletedAt { get; set; }
        public int Progress { get; set; } = 0;
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
