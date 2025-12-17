using Core.Enums;
using Core.Models.Courses;
using Core.Models.Learning;


namespace Core.Models.Lessons
{
    public class Lesson
    {
        public Lesson()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }                       // المعرف الفريد للدرس
        public string Title { get; set; }                  // عنوان الدرس
        public string Description { get; set; }            // وصف الدرس
        public string Content { get; set; }                // محتوى الدرس (نص/HTML)
        public string? VideoUrl { get; set; }               // رابط الفيديو
        public string? AttachmentUrl { get; set; }         // رابط المرفقات
        public int Duration { get; set; }                  // المدة (دقائق)
        public int Order { get; set; }                     // ترتيب العرض
        public bool IsFree { get; set; } = false;          // درس مجاني للمعاينة
        public LessonType Type { get; set; }               // نوع الدرس
        public int OrderInCourse { get; set; }             // ترتيب الدرس في الكورس


        // العلاقات
        public Guid CourseId { get; set; }                 // معرف الكورس
        public Course Course { get; set; }                 // الكورس


    }

}