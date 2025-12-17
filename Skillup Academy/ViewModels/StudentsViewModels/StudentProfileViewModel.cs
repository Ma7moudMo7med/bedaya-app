namespace Skillup_Academy.ViewModels.StudentsViewModels
{
    public class StudentProfileViewModel
    {
        // ----------- Identity بيانات الـ -----------

        public Guid Id { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // ----------- بيانات الحساب -----------

        public string? FullName { get; set; }

        // الصورة ممكن تكون null → هنعالجها في الـ Controller
        public string? ProfilePicture { get; set; } //= "~/img/Profile/download.png";

        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastProfileUpdate { get; set; }

        public bool IsActive { get; set; }
        public bool CanViewPaidCourses { get; set; }

        // ----------- إحصائيات الطالب -----------

        public int CompletedCourses { get; set; }
        public int TotalEnrollments { get; set; }

        public int OverallProgress { get; set; }

        // ----------- اشتراك الطالب -----------

        public bool HasActiveSubscription { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
    }
}
