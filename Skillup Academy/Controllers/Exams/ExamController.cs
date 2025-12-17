using AutoMapper;
using Core.Models.Courses;
using Core.Models.Exams;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Skillup_Academy.ViewModels.ExamsViewModels;
using System.Threading.Tasks;

namespace Skillup_Academy.Controllers.Exams
{
    public class ExamController : Controller
    {
        private readonly IRepository<Exam> repository;
        private readonly IRepository<Course> courseRepository;
        private readonly IMapper mapper;

        public ExamController(IRepository<Exam> repository, IRepository<Course> courseRepository, IMapper mapper)
        {
            this.repository = repository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        // /Exam/index
        public async Task<IActionResult> Index()
        {
            var exams = await repository.GetAllAsync();
            return View(exams);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var courses = await courseRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title"); // هنستخدمها في الـ View
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var courses = await courseRepository.GetAllAsync();
                ViewBag.Courses = new SelectList(courses, "Id", "Title");
                return View(model);
            }

            var exam = mapper.Map<Exam>(model);
            await repository.AddAsync(exam);
            await repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var exam = await repository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            var model = mapper.Map<ExamViewModel>(exam);
            var courses = await courseRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title", model.CourseId); // هنستخدمها في الـ View
            return View(model);
        }
        #region Edit
        [HttpPost]
        //public async Task<IActionResult> Edit(Guid id,ExamViewModel NewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var oldExam = await repository.GetByIdAsync(id);
        //        if (oldExam == null)
        //        {
        //            return NotFound();
        //        }
        //        // var model = mapper.Map(NewModel, oldExam);
        //        oldExam.Title = NewModel.Title;
        //        oldExam.Description = NewModel.Description;
        //        oldExam.Duration = NewModel.Duration;
        //        oldExam.TotalQuestions = NewModel.TotalQuestions;
        //        oldExam.PassMark = NewModel.PassMark;
        //        oldExam.MaxAttempts = NewModel.MaxAttempts;
        //        //oldExam.IsPublished = NewModel.IsPublished;
        //        oldExam.AvailableFrom = NewModel.AvailableFrom;
        //        oldExam.AvailableTo = NewModel.AvailableTo;
        //        oldExam.CourseId = NewModel.CourseId;
        //        if (oldExam.IsPublished == false && NewModel.IsPublished == true)
        //        {
        //            oldExam.IsPublished = true;
        //        }
        //        oldExam.IsPublished = NewModel.IsPublished;
        //        repository.Update(oldExam);
        //        await repository.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(NewModel);

        //} 
        #endregion

        [HttpPost]
        public async Task<IActionResult> Edit(ExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var courses = await courseRepository.GetAllAsync();
                ViewBag.Courses = new SelectList(courses, "Id", "Title", model.CourseId);
                return View(model);
            }
            var exam = await repository.GetByIdAsync(model.Id);
            if (exam == null)
            {
                return NotFound();
            }
            mapper.Map(model, exam);
            repository.Update(exam);
            await repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exam = await repository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
           return View(exam);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var exam = await repository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            repository.Delete(exam);
            await repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var exam = await repository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            var model = mapper.Map<ExamViewModel>(exam);
            return View(model);
        }
    }
}
