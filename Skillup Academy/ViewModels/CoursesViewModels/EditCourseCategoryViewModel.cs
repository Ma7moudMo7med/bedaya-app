using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.CoursesViewModels
{
	public class EditCourseCategoryViewModel
	{
		public Guid Id { get; set; } 
		public string Name { get; set; } 
		public string Description { get; set; } 
		public string? ImageOld { get; set; }
		public IFormFile Image { get; set; } 
		public bool IsActive { get; set; }  
	}
}
