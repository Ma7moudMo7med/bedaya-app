using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Users;

namespace Core.Models.Learning
{
	public class Answer
	{
		public Answer()
		{
			Id = Guid.NewGuid();
		}
		public Guid Id { get; set; }                       // المعرف الفريد للإجابة
		public string Content { get; set; }                // محتوى الإجابة
		public DateTime AnsweredDate { get; set; } = DateTime.Now; // تاريخ الإجابة
		public bool IsCorrect { get; set; } = false;       // إجابة صحيحة (للأسئلة التعليمية)
		public bool IsAccepted { get; set; } = false;      // مقبولة من صاحب السؤال


		// العلاقات
		public Guid QuestionId { get; set; }               // السؤال المرتبط
		public Question Question { get; set; }             // السؤال
		public Guid UserId { get; set; }                 // المجيب
		public User User { get; set; }                     // المجيب


	}
}
