using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Courses;
using Core.Models.Learning;

namespace Core.Models.Exams
{
	public class Exam
	{
		public Exam()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للامتحان
		public string Title { get; set; }                  // عنوان الامتحان
		public string Description { get; set; }            // وصف الامتحان

		// إعدادات الامتحان
		public int Duration { get; set; }                  // المدة (دقائق)
		public int TotalQuestions { get; set; }            // عدد الأسئلة
		public int PassMark { get; set; }                  // درجة النجاح
		public int MaxAttempts { get; set; } = 1;          // عدد المحاولات المسموحة
		public bool IsPublished { get; set; } = false;     // منشور/غير منشور

		// التواريخ
		public DateTime CreatedDate { get; set; } = DateTime.Now; // تاريخ الإنشاء
		public DateTime? AvailableFrom { get; set; }       // متاح من تاريخ
		public DateTime? AvailableTo { get; set; }         // متاح إلى تاريخ

		// العلاقات	
		public Guid CourseId { get; set; }                 // الكورس المرتبط
		public Course Course { get; set; }                 // الكورس

		public ICollection<Question> Questions { get; set; } // أسئلة الامتحان
		public ICollection<ExamAttempt> ExamAttempts { get; set; } // محاولات الأداء
	}
}
