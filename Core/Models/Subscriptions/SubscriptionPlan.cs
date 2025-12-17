using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Courses;
using Core.Enums;
using Core.Models.Users;

namespace Core.Models.Subscriptions
{
	public class SubscriptionPlan
	{
		public SubscriptionPlan()
		{
			Id = Guid.NewGuid();
		}
		public Guid Id { get; set; }                       // المعرف الفريد للاشتراك

		// حالة الاشتراك
		public DateTime StartDate { get; set; }            // تاريخ البدء
		public DateTime EndDate { get; set; }              // تاريخ الانتهاء
		public bool IsActive { get; set; } = true;         // نشط/منتهي
		public int MaxCourses { get; set; }                // أقصى عدد كورسات
		// الدفع
		public decimal PaidAmount { get; set; }            // المبلغ المدفوع
		public string TransactionId { get; set; }          // رقم المعاملة


		// العلاقات
		public Guid UserId { get; set; }                 // المستخدم
		public User User { get; set; }                     // المستخدم
		public Guid SubscriptionId { get; set; }           // الخطة
		public Subscription Subscription { get; set; }     // الخطة
		public Guid? CourseId { get; set; }                // الكورس (لو كان اشتراك في كورس محدد)
		public Course Course { get; set; }                 // الكورس


	}
}
