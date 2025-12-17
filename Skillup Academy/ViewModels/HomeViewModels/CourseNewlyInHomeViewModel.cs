namespace Skillup_Academy.ViewModels.HomeViewModels
{
	public class CourseNewlyInHomeViewModel
	{
		public Guid Id { get; set; }

		public string ShortDescription { get; set; }
		public string Title { get; set; }    
		
		public int TotalDuration { get; set; }            
		public double AverageRating { get; set; }   
		
		public string Image { get; set; }
		public bool IsFree { get; set; }
		public string TeacherName { get; set; }
		public string CategoryName { get; set; }

	}
}
