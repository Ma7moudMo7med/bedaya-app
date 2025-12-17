using Core.Enums;

namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class StudentCourseVM
    {
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImage { get; set; }
        public DateTime EnrolledAt { get; set; }
        public StudentStatus Status { get; set; } // Active / Completed / Dropped
    }
}
