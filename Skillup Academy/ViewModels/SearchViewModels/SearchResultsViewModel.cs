using Core.Models.Courses;

namespace Skillup_Academy.ViewModels.SearchViewModels
{
	public class SearchResultsViewModel
	{
		// النتائج المعروضة في الصفحة الحالية
		public IEnumerable<Course> Courses { get; set; }

		// قوائم التصفية (نستخدمها لملء الشريط الجانبي)
		public IEnumerable<CourseCategory> Categories { get; set; }
		public IEnumerable<SubCategory> SubCategories { get; set; }

		// حالة البحث الحالية
		public string? Query { get; set; }
		public List<Guid>? SelectedCategoryIds { get; set; } // تم تغيير النوع لـ Guid
		public bool IsFree { get; set; }
 
		// معلومات التقسيم (Pagination)
		public int CurrentPage { get; set; }
		public int PageSize { get; set; } = 9; // تم تحديدها 9 كورسات في الصفحة
		public int TotalResults { get; set; }
		public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
	}

}
