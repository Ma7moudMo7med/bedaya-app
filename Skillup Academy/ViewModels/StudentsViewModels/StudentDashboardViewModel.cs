namespace Skillup_Academy.ViewModels.StudentsViewModels
{
    public class StudentDashboardViewModel
    {
        public string StudentName { get; set; } = "mohamed";
        public int TotalCourses { get; set; } = 4;
        public int CompletedCourses { get; set; } = 2;
        public int OverallProgress { get; set; } = 80;
        public int CertificatesEarned { get; set; } = 2;
        public List<StudentCoursesViewModel> Courses { get; set; }
    }
}
