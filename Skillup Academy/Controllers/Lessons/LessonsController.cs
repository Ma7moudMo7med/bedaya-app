using System.Threading.Tasks;
using AutoMapper;
using Core.Enums;
using Core.Interfaces;
using Core.Models.Courses;
using Core.Models.Enrollments;
using Core.Models.Lessons;
using Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.ViewModels.LessonsViewModels;

namespace Skillup_Academy.Controllers.Lessons
{
    [Authorize(Roles = "Admin,Instructor")]
    public class LessonsController : Controller
    {
        private readonly IRepository<Lesson> _repoLesson;
        private readonly IRepository<Course> _repoCourses;
		private readonly IMapper _mapper;
        private readonly SaveImage _saveImage;
        private readonly DeleteImage _deleteImage;
		private readonly IRepository<Enrollment> _repoEnrollment;
		private readonly UserManager<User> _userManager;
		private readonly IRepository<Student> _repoStudent;

		public LessonsController(IRepository<Lesson> repository, IMapper mapper, IRepository<Course> repoCourses
            , SaveImage saveImage, DeleteImage deleteImage,IRepository<Enrollment> repoEnrollment,
            UserManager<User> userManager,IRepository<Student> repoStudent)
        {
			_repoLesson = repository;
			_mapper = mapper;
			_repoCourses = repoCourses;
            _saveImage = saveImage;
            _deleteImage = deleteImage;
			_repoEnrollment = repoEnrollment;
			_userManager = userManager;
			_repoStudent = repoStudent;
		}
        // commat

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
			var lessons = _repoLesson.Query()
				.Where(i => i.CourseId == id)
				.OrderBy(i => i.OrderInCourse)
				.ToList();

			var userId = _userManager.GetUserId(User);

            if (userId == null) 
                return RedirectToAction("Index", "Home");

			var founded = _repoEnrollment.Query() 
                .Where(e => e.CourseId == id && e.StudentId == Guid.Parse(userId))
                .FirstOrDefault();

            if (founded==null)
            {
                Enrollment enrollment = new Enrollment 
                { 
                    CourseId = id,
                    StudentId = Guid.Parse(userId),
                    Status = StudentStatus.Active,
                    EnrolledAt = DateTime.Now,
                    CompletedAt=DateTime.Now
			    };
                await _repoEnrollment.AddAsync(enrollment);

				if (!Guid.TryParse(userId, out var userGuid))
				{
 					return RedirectToAction("Index", "Home");
				}
                if (User.IsInRole("Student")) 
                { 
					var student = await _repoStudent.GetByIdAsync(userGuid);
					student.TotalEnrollments += 1;
				} 

				await _repoEnrollment.SaveChangesAsync();
            }

			ViewBag.courseId = id;
			return View(lessons);
        }

		[AllowAnonymous]
		[HttpGet]
        public IActionResult Details(Guid id)
        { 
            var lesson = _repoLesson.Query().Include(c => c.Course).FirstOrDefault(i=>i.Id==id);

            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }


        [HttpGet]
        public async Task<IActionResult> Create(Guid coursId)
        { 
			ViewBag.Courses =new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
            CreateLessonViewModel model = new CreateLessonViewModel
            {
                CourseId=coursId
            };
 			return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLessonViewModel lesson)
        {
            if (ModelState.IsValid)
            { 
                var lessonEntity = _mapper.Map<Lesson>(lesson);
                 lessonEntity.Order=lesson.OrderInCourse;

                lessonEntity.VideoUrl = await _saveImage.SaveImgAsync(lesson.VideoUrl);
                lessonEntity.AttachmentUrl = await _saveImage.SaveImgAsync(lesson.AttachmentUrl);

                await _repoLesson.AddAsync(lessonEntity);

                var course = await _repoCourses.GetByIdAsync(lessonEntity.CourseId);
                course.TotalLessons += 1;
                course.TotalDuration += lesson.Duration;

                await _repoLesson.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id=lesson.CourseId});
            }
			ViewBag.Courses = new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
			return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        { 
            var lesson = await _repoLesson.GetByIdAsync(id);

			var lessonEntity = _mapper.Map<EditLessonViewModel>(lesson);

			if (lesson == null)
            {
                return NotFound();
            }
 			ViewBag.Courses = new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
			return View(lessonEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditLessonViewModel lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldLesson =  await _repoLesson.GetByIdAsync(id);

                oldLesson.Id = id;
                oldLesson.Order = lesson.OrderInCourse;

                if (lesson.VideoUrlFile != null)
                    oldLesson.VideoUrl = await _saveImage.SaveImgAsync(lesson.VideoUrlFile);

                if (lesson.AttachmentUrlFile != null)
                    oldLesson.AttachmentUrl = await _saveImage.SaveImgAsync(lesson.AttachmentUrlFile);
                oldLesson.Title = lesson.Title;
                oldLesson.Description = lesson.Description;
                oldLesson.Content = lesson.Content;
                oldLesson.IsFree = lesson.IsFree;
   
                var course = await _repoCourses.GetByIdAsync(oldLesson.CourseId);
				course.TotalDuration -= oldLesson.Duration;

				course.TotalDuration += lesson.Duration;
                  
				_repoLesson.Update(oldLesson);
                await _repoLesson.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index), new { id = lesson.CourseId });
            }
			ViewBag.Courses = new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
			return View(lesson);
        }


		[HttpGet]
		public async Task<IActionResult> Delete(Guid id)
		{
			var lesson = await _repoLesson.GetByIdAsync(id);
			if (lesson == null)
			{
				return NotFound();
			}

			return View(lesson);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var lesson = await _repoLesson.GetByIdAsync(id);
            Guid courseId = lesson.CourseId;
            if (lesson != null)
			{
				_repoLesson.Delete(lesson);

				var course = await _repoCourses.GetByIdAsync(lesson.CourseId);
				course.TotalLessons -= 1;
				course.TotalDuration -= lesson.Duration;

			}
            await _repoLesson.SaveChangesAsync();
            _deleteImage.DeleteImg(lesson.VideoUrl);
            _deleteImage.DeleteImg(lesson.AttachmentUrl);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index), new { id = courseId });
		}

         
    }
}
