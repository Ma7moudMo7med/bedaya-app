using Core.Enums;
using Core.Models.Enrollments;
using Core.Models.Exams;

namespace Core.Models.Users
{
	public class Student : User
	{ 

		// الإحصائيات
		public int CompletedCourses { get; set; }          // عدد الكورسات المكتملة
		public int TotalEnrollments { get; set; }          // إجمالي الاشتراكات
		public StudentStatus StudentStatus { get; set; }


		// العلاقات الخاصة
		public ICollection<ExamAttempt> ExamAttempt { get; set; }       // الامتحانات
		public ICollection<Enrollment> Enrollments { get; set; }       // التسجيلات

    }
}
