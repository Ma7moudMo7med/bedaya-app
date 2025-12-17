using Core.Enums;

namespace Skillup_Academy.ViewModels.SubscriptionsViewModels
{
	public class EditSubscriptionViewModel
	{
		public Guid Id { get; set; }                      
		public string Name { get; set; }                 
		public string Description { get; set; }            
		public decimal Price { get; set; }            
		public SubscriptionType Type { get; set; }        
		public int DurationDays { get; set; }              
		public int MaxCourses { get; set; }                 
		public bool IsActive { get; set; } = true;         

	}
}
