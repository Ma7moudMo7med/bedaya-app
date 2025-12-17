using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.LearningViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsResolved { get; set; } = false;

        // العلاقات المحتملة (اختيارية)
        public Guid? LessonId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? ExamId { get; set; }

        public Guid UserId { get; set; }

        // ↓↓↓ دي هنستخدمها في الـ dropdowns داخل الـ View ↓↓↓
        public IEnumerable<SelectListItem>? Lessons { get; set; }
        public IEnumerable<SelectListItem>? Courses { get; set; }
        public IEnumerable<SelectListItem>? Exams { get; set; }
    }
}
