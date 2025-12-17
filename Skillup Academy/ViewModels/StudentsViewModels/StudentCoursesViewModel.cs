namespace Skillup_Academy.ViewModels.StudentsViewModels
{
    public class StudentCoursesViewModel
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalLessons { get; set; }
        public int Progress { get; set; }   // نسبة التقدم في الكورس
        public bool IsCompleted { get; set; }
        public bool HasCertificate { get; set; }
    }
}
