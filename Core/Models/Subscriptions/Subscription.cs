using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.Subscriptions
{
	public class Subscription
	{
		public Subscription()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للخطة
		public string Name { get; set; }                   // اسم الخطة
		public string Description { get; set; }            // وصف الخطة
		public decimal Price { get; set; }                 // السعر
		public SubscriptionType Type { get; set; }         // نوع الخطة
		public int DurationDays { get; set; }              // المدة بالأيام
		public int MaxCourses { get; set; }                // أقصى عدد كورسات
		public bool IsActive { get; set; } = true;         // مفعلة/غير مفعلة

		// العلاقات
		public ICollection<SubscriptionPlan> Subscribes { get; set; } // الاشتراكات
	}
}
