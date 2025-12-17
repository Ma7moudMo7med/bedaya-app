using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Users;

namespace Core.Models.Exams
{
	public class ExamAttempt
	{
		public ExamAttempt()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للمحاولة
		public DateTime StartTime { get; set; }            // وقت البدء
		public DateTime? EndTime { get; set; }             // وقت الانتهاء
		public int Score { get; set; }                     // الدرجة
		public int TotalQuestions { get; set; }            // إجمالي الأسئلة
		public int CorrectAnswers { get; set; }            // الإجابات الصحيحة
		public bool IsPassed { get; set; } = false;        // نجح/رسب
		public int AttemptNumber { get; set; }             // رقم المحاولة

		// العلاقات		
		public Guid ExamId { get; set; }                   // الامتحان
		public Exam Exam { get; set; }                     // الامتحان
		public Guid StudentId { get; set; }              // الطالب
		public Student Student { get; set; }               // الطالب



	}
}
