using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Learning;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.ExamsViewModels
{
    public class ExamViewModel
    {
       
        public Guid Id { get; set; }                     // معرف الامتحان
        public string Title { get; set; }                  // عنوان الامتحان
        public string Description { get; set; }            // وصف الامتحان

        // إعدادات الامتحان
        public int Duration { get; set; }                  // المدة (دقائق)
        public int TotalQuestions { get; set; }            // عدد الأسئلة
        public int PassMark { get; set; }                  // درجة النجاح
        public int MaxAttempts { get; set; } = 1;          // عدد المحاولات المسموحة
        public bool IsPublished { get; set; } = false;     // منشور/غير منشور
        public DateTime? AvailableFrom { get; set; }       // متاح من تاريخ
        public DateTime? AvailableTo { get; set; }         // متاح إلى تاريخ

        // العلاقات	
        public Guid? CourseId { get; set; }                 // الكورس المرتبط
        public IEnumerable<SelectListItem>? Courses { get; set; } // للـ dropdown list

    }
}
