using Core.Models.Exams;
using Core.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.ExamsViewModels
{
    public class ExamAttemptViewModel
    {
        public Guid Id { get; set; }                       // المعرف الفريد للمحاولة
        public DateTime StartTime { get; set; }            // وقت البدء
        public DateTime? EndTime { get; set; }             // وقت الانتهاء
        public int Score { get; set; }                     // الدرجة
        public int TotalQuestions { get; set; }            // إجمالي الأسئلة
        public int CorrectAnswers { get; set; }            // الإجابات الصحيحة
        public bool IsPassed { get; set; } = false;        // نجح/رسب
        public int AttemptNumber { get; set; }             // رقم المحاولة

        // العلاقات		
        public Guid ExamId { get; set; }                   // الامتحان
        public SelectList Exams { get; set; }                     // الامتحان
        public Guid StudentId { get; set; }              // الطالب
        public SelectList Students { get; set; }               // الطالب
    }
}
