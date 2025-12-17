using Core.Enums;

namespace Skillup_Academy.ViewModels.LessonsViewModels
{
	public class EditLessonViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string? Content { get; set; }
		public string? VideoUrl { get; set; }
		public string? AttachmentUrl { get; set; }
		public IFormFile? VideoUrlFile { get; set; }
		public IFormFile? AttachmentUrlFile { get; set; }

		public int Duration { get; set; }
		public bool IsFree { get; set; } = false;
		public int OrderInCourse { get; set; }
		public LessonType Type { get; set; }               // نوع الدرس

		public Guid CourseId { get; set; }                 // معرف الكورس
	}
}
