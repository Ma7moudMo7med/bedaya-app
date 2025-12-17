using AutoMapper;
using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Learning;
using Core.Models.Lessons;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Skillup_Academy.ViewModels.LearningViewModels;

namespace Skillup_Academy.Controllers.Learning
{
    public class QuestionController : Controller
    {
        private readonly IRepository<Question> repository;
        private readonly IRepository<Course>  courseRepository;
        private readonly IRepository<Lesson> lessonRepository;
        private readonly IRepository<Exam> examRepository;
        private readonly IMapper mapper;

        public QuestionController(
        IRepository<Question> repository,
        IRepository<Course> courseRepository,
        IRepository<Lesson> lessonRepository,
        IRepository<Exam> examRepository,
        IMapper mapper)
        {
            this.repository = repository;
            this.courseRepository = courseRepository;
            this.lessonRepository = lessonRepository;
            this.examRepository = examRepository;
            this.mapper = mapper;
        }
        // /Question/index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = await repository.GetAllAsync();
            var model = mapper.Map<IEnumerable<QuestionListViewModel>>(questions);
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var courses = await courseRepository.GetAllAsync();
            var lessons = await lessonRepository.GetAllAsync();
            var exams = await examRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            ViewBag.Lessons = new SelectList(lessons, "Id", "Title");
            ViewBag.Exams = new SelectList(exams, "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // لازم نرجّع البيانات تاني لو الفورم فيه Error
                var courses = await courseRepository.GetAllAsync();
                var lessons = await lessonRepository.GetAllAsync();
                var exams = await examRepository.GetAllAsync();
                ViewBag.Courses = new SelectList(courses, "Id", "Title");
                ViewBag.Lessons = new SelectList(lessons, "Id", "Title");
                ViewBag.Exams = new SelectList(exams, "Id", "Title");

                return View(model);
            }

            var question = mapper.Map<Question>(model);
            await repository.AddAsync(question);
            await repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var question = await repository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            var model = mapper.Map<QuestionViewModel>(question);
            var courses = await courseRepository.GetAllAsync();
            var lessons = await lessonRepository.GetAllAsync();
            var exams = await examRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            ViewBag.Lessons = new SelectList(lessons, "Id", "Title");
            ViewBag.Exams = new SelectList(exams, "Id", "Title");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var question = mapper.Map<Question>(model);
                repository.Update(question);
                await repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var courses = await courseRepository.GetAllAsync();
            var lessons = await lessonRepository.GetAllAsync();
            var exams = await examRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            ViewBag.Lessons = new SelectList(lessons, "Id", "Title");
            ViewBag.Exams = new SelectList(exams, "Id", "Title");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var question = await repository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            var model = mapper.Map<QuestionViewModel>(question);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var question = await repository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            repository.Delete(question);
            await repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var question = await repository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            var model = mapper.Map<QuestionViewModel>(question);
            return View(model);
        }
    }
}
