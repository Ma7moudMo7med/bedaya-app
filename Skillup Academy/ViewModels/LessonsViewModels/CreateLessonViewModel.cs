using Core.Models.Courses;
using Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.LessonsViewModels
{
		public class CreateLessonViewModel
		{
		    public Guid Id { get; set; }                       // المعرف الفريد للدرس
            public string Title { get; set; }              
			public string Description { get; set; }           
			public string? Content { get; set; }
			//public string? VideoUrl { get; set; }                
			//public string? AttachmentUrl { get; set; }          
			public IFormFile? VideoUrl { get; set; }
			public IFormFile? AttachmentUrl { get; set; }
			public int Duration { get; set; }                  
 			public bool IsFree { get; set; } = false;          
 			public int OrderInCourse { get; set; }
			public LessonType Type { get; set; }               // نوع الدرس

			public Guid CourseId { get; set; }                 // معرف الكورس
 
		}
}
