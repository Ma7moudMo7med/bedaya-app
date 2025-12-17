using Core.Models.Courses;

namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class TeacherDashboardVM
    {
       
        public string TeacherName { get; set; }
        public string TeacherBio { get; set; }
        public string TeacherExpertise { get; set; }
        public string ProfilePictureUrl { get; set; }
        public Decimal TeacherRating { get; set; }

        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLessons { get; set; } = 0;
        public List<CourseDashboardViewModel> Courses { get; set; }
    }
}
