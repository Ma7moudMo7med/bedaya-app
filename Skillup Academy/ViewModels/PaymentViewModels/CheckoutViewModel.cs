namespace Skillup_Academy.ViewModels.PaymentViewModels
{
	public class CheckoutViewModel
	{
		public Guid PlanId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int DurationDays { get; set; }
		public int MaxCourses { get; set; }
		public string? TypeMethod { get; set; }

		public string FormattedPrice => Price.ToString("C");
	}
}
