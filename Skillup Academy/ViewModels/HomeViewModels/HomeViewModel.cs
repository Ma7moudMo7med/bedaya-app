using Core.Models.Courses;

namespace Skillup_Academy.ViewModels.HomeViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<CourseCategoryInHomeViewModel> ListCategory { get; set; }
		public IEnumerable<CourseNewlyInHomeViewModel> NewlyAdded { get; set; }
		public IEnumerable<CourseNewlyInHomeViewModel> MostPopular { get; set; }

	}
}
