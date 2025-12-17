using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Courses
{
	public class SubCategory
	{
		public SubCategory()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للتصنيف الفرعي
		public string Name { get; set; }                   // الاسم
		public string Description { get; set; }            // الوصف
		public bool IsActive { get; set; } = true;         // مفعل/غير مفعل

		// العلاقات		
		public Guid CategoryId { get; set; }               // التصنيف الرئيسي
		public CourseCategory Category { get; set; }       // التصنيف الرئيسي

		public ICollection<Course> Courses { get; set; }   // الكورسات
	}
}
