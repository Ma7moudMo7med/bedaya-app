namespace Skillup_Academy.ViewModels.StudentsViewModels
{
    public class StudentSubscriptionViewModel
    {
        public string? SubscriptionName { get; set; }     // اسم الخطة
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public int MaxCourses { get; set; }
        public decimal PaidAmount { get; set; }

        public string? TransactionId { get; set; }
    }
}
