using AutoMapper;
using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Learning;
using Core.Models.Lessons;
using Skillup_Academy.ViewModels.CoursesViewModels;
using Skillup_Academy.ViewModels.ExamsViewModels;
using Skillup_Academy.ViewModels.LearningViewModels;
using Skillup_Academy.ViewModels.LessonsViewModels;
namespace Skillup_Academy.Mappings
{
	public class MappingProfile: Profile
	{
		public MappingProfile()
		{

			CreateMap<Course, CreateCourseViewModel>().ReverseMap();
			CreateMap<Lesson, CreateLessonViewModel>().ReverseMap();
			CreateMap<Course, EditCourseViewModel>().ReverseMap();
			CreateMap<Lesson, EditLessonViewModel>().ReverseMap();
            CreateMap<ExamViewModel, Exam>().ReverseMap();
            CreateMap<QuestionViewModel, Question>().ReverseMap();

            CreateMap<Question, QuestionListViewModel>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : ""))
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : ""))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : ""));

            CreateMap<Answer, AnswerViewModel>().ReverseMap();


            CreateMap<Course, Course>().ReverseMap();
		    // الشرح 
			//CreateMap<Notification, NotificationDTO>();
			//CreateMap<NotificationDTO, Notification>();

			//CreateMap<Notificationv, NotificationDTO>().ReverseMap();
			 
			//طريقه الاستخدام 
			//private readonly IMapper _mapper;
			//var entity = _mapper.Map<Notification>(NotificationDTO);      Notification  كده هتكون ك 



		}
	}
}
