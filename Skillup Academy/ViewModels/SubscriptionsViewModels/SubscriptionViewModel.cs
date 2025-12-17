using Core.Enums;

namespace Skillup_Academy.ViewModels.SubscriptionsViewModels
{
	public class SubscriptionViewModel
	{
		public string Name { get; set; }                   // اسم الخطة
		public string Description { get; set; }            // وصف الخطة
		public decimal Price { get; set; }                 // السعر
		public SubscriptionType Type { get; set; }         // نوع الخطة
		public int DurationDays { get; set; }              // المدة بالأيام
		public int MaxCourses { get; set; }                // أقصى عدد كورسات
		public bool IsActive { get; set; } = true;         // مفعلة/غير مفعلة

	}
}
