using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Courses;
using Core.Models.Users;

namespace Core.Models.Reviews
{
	public class CourseReview
	{
		public CourseReview()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للتقييم
		public int Rating { get; set; }                    // التقييم (1-5)
		public string Comment { get; set; }                // التعليق
		public DateTime ReviewDate { get; set; } = DateTime.Now; // تاريخ التقييم

		// التقييم التفصيلي
		public int ContentRating { get; set; }             // تقييم المحتوى
		public int TeachingRating { get; set; }            // تقييم الشرح
 
		// الموافقة
		public bool IsApproved { get; set; } = false;      // موافق عليه/غير موافق


		// العلاقات	
		public Guid CourseId { get; set; }                 // الكورس
		public Course Course { get; set; }                 // الكورس

		public Guid UserId { get; set; }                 // المستخدم
		public User User { get; set; }                     // المستخدم
	
	
	}
}
