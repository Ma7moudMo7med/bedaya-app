using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Enums;

namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class StudentDetailsVM
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public int CoursesCount { get; set; }
        public StudentStatus Status { get; set; } // Active / Inactive

        public ICollection<StudentCourseDTO> Courses { get; set; } = new List<StudentCourseDTO>();
    }
}
