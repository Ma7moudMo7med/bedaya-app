using Core.Models.Exams;
using Core.Models.Reviews;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Core.Models.Lessons;
using Core.Models.Enrollments;


namespace Core.Models.Courses
{
    public class Course
    {
        public Course()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }                       // المعرف الفريد للكورس
        public string Title { get; set; }                  // عنوان الكورس
        public string Description { get; set; }            // الوصف الكامل
        public string ShortDescription { get; set; }       // وصف مختصر
        public string ThumbnailUrl { get; set; }           // صورة الغلاف
        public string PreviewVideoUrl { get; set; }        // فيديو المعاينة

        public bool IsFree { get; set; } = false;          // مجاني/مدفوع

        // الحالة
        public bool IsPublished { get; set; } = false;     // منشور/غير منشور

        // التواريخ
        public DateTime CreatedDate { get; set; } = DateTime.Now; // تاريخ الإنشاء
        public DateTime? UpdatedDate { get; set; }         // تاريخ التحديث
        public DateTime? PublishedDate { get; set; }       // تاريخ النشر

        // الإحصائيات
        public int TotalEnrollments { get; set; }          // عدد المسجلين
        public double AverageRating { get; set; }          // متوسط التقييم

        public int TotalLessons { get; set; }              // عدد الدروس
        public int TotalDuration { get; set; }             // المدة الإجمالية (دقائق)

        // العلاقات
        public Guid TeacherId { get; set; }              // المدرس
        public Teacher Teacher { get; set; }               // كائن المدرس
        public Guid? CategoryId { get; set; }               // التصنيف الرئيسي
        public CourseCategory? Category { get; set; }       // التصنيف الرئيسي
        public Guid? SubCategoryId { get; set; }           // التصنيف الفرعي
        public SubCategory? SubCategory { get; set; }       // التصنيف الفرعي

        public ICollection<Enrollment> Enrollments { get; set; }       // التسجيلات
        public ICollection<Lesson> Lessons { get; set; } // دروس الكورس
        public ICollection<CourseReview> CourseReviews { get; set; } // التقييمات
        public ICollection<SubscriptionPlan> Subscribes { get; set; }      // الاشتراكات
        public ICollection<Exam> Exams { get; set; }                // الامتحانات
    }

}