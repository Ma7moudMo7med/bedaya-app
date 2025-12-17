using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.LearningViewModels
{
    public class AnswerViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime AnsweredDate { get; set; }

        // العلاقات
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }

        // Dropdowns
        public IEnumerable<SelectListItem>? Questions { get; set; }
        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}
