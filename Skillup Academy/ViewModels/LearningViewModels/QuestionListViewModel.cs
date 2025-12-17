namespace Skillup_Academy.ViewModels.LearningViewModels
{
    public class QuestionListViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CourseTitle { get; set; }
        public string LessonTitle { get; set; }
        public string ExamTitle { get; set; }
        public DateTime AskedDate { get; set; }
        public bool IsResolved { get; set; }
    }
}
