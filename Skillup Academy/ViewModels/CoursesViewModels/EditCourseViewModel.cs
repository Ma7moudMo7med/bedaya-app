namespace Skillup_Academy.ViewModels.CoursesViewModels
{
	public class EditCourseViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ShortDescription { get; set; } 
		public bool IsFree { get; set; } = false;
		// الحالة
		public bool IsPublished { get; set; } = false;

		public string ThumbnailUrl { get; set; }
		public string PreviewVideoUrl { get; set; }
		
		public IFormFile? ThumbnailUrlFile { get; set; }
		public IFormFile? PreviewVideoFile { get; set; }

		// الإحصائيات	
		public int TotalLessons { get; set; }            // عدد الدروس
		public int TotalDuration { get; set; }           // المدة الإجمالية (دقائق)

		public DateTime CreatedDate { get; set; }          // تاريخ الإنشاء
		public DateTime? UpdatedDate { get; set; }         // تاريخ التحديث
		public DateTime? PublishedDate { get; set; }       // تاريخ النشر

		public Guid? SubCategoryId { get; set; }           // التصنيف الفرعي
		public Guid? CategoryId { get; set; }               // التصنيف الرئيسي

	}
}
