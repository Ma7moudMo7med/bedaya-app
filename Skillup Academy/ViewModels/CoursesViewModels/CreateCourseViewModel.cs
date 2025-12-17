using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Lessons;
using Core.Models.Reviews;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.CoursesViewModels
{
	public class CreateCourseViewModel
	{
		public string Title { get; set; }                   
		public string Description { get; set; }            
		public string ShortDescription { get; set; }        
		public IFormFile ThumbnailUrl { get; set; }
        public IFormFile PreviewVideo { get; set; }         

		public bool IsFree { get; set; } = false;           

		// الحالة
		public bool IsPublished { get; set; } = false;      
 
		// الإحصائيات
		public int TotalEnrollments { get; set; } = 0;          // عدد المسجلين
		public double AverageRating { get; set; } = 0;         // متوسط التقييم
		public int TotalLessons { get; set; } = 0;         // عدد الدروس
		public int TotalDuration { get; set; } = 0;       // المدة الإجمالية (دقائق)

		public Guid? SubCategoryId { get; set; }           // التصنيف الفرعي
		public Guid? CategoryId { get; set; }               // التصنيف الرئيسي

	}
}
