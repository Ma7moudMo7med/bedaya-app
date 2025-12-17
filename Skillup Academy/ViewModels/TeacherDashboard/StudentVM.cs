using Core.Enums;

namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class StudentVM
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Course info for list view
        public string CourseTitle { get; set; }
        public int CoursesCount { get; set; }
        public StudentStatus Status { get; set; }
        //public string CourseProgress { get; set; }
    }
}
