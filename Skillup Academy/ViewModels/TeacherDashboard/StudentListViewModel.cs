using Core.DTOs.TeacherDashboardDTOs;
using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Enums;

namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class StudentListViewModel
    {
        public List<StudentVM> Students { get; set; } = new();

        // Paging
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        // Filters
        public string? SearchQuery { get; set; }
        public Guid? SelectedCourseId { get; set; }
        public StudentStatus? SelectedStatus { get; set; }

        // Dashboard stats
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public int CompletedStudents { get; set; }

        // Courses owned by teacher
        public List<CourseDashboardDTO> TeacherCourses { get; set; } = new();
    }
}
