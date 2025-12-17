using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Courses
{
	public class CourseCategory
	{
		public CourseCategory()
		{
			Id = Guid.NewGuid();
		}

 		public Guid Id { get; set; }                       // المعرف الفريد للتصنيف
		public string Name { get; set; }                   // اسم التصنيف
		public string Description { get; set; }            // وصف التصنيف
		public string Icon { get; set; }                   // أيقونة
 		public bool IsActive { get; set; } = true;         // مفعل/غير مفعل
 
		// العلاقات
		public ICollection<SubCategory> SubCategories { get; set; } // التصنيفات الفرعية
		public ICollection<Course> Courses { get; set; }            // الكورسات
	}
}
