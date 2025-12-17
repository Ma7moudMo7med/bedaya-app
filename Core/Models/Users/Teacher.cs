using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Courses;

namespace Core.Models.Users
{
	public class Teacher : User
	{
        //public Guid Id { get; set; }
        // بيانات خاصة بالمدرس
        public string Bio { get; set; }                    // السيرة الذاتية
		public string Qualifications { get; set; }         // المؤهلات العلمية
		public string Expertise { get; set; }              // التخصص
		//public int Experience { get; set; }              // الخبرة
        public bool IsApproved { get; set; } = false;      // موافقة الأدمن
		public DateTime? ApprovalDate { get; set; }        // تاريخ الموافقة

		// الإحصائيات
		public int TotalStudents { get; set; }             // إجمالي الطلاب
		public decimal Rating { get; set; }                // متوسط التقييم
		public int TotalCourses { get; set; }              // عدد الكورسات

		// العلاقات الخاصة
		public ICollection<Course> Courses { get; set; }   // الكورسات

	}
}
