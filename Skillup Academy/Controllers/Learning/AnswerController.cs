using AutoMapper;
using Core.Models.Learning;
using Core.Models.Users;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Skillup_Academy.ViewModels.LearningViewModels;

namespace Skillup_Academy.Controllers.Learning
{
    public class AnswerController : Controller
    {
        private readonly IRepository<Answer> repository;
        private readonly IRepository<Question> questionRepository;
        private readonly IRepository<User> userRepository;
        private readonly IMapper mapper;

        public AnswerController(
            IRepository<Answer> repository,
            IRepository<Question> questionRepository,
            IRepository<User> userRepository,
            IMapper mapper)
        {
            this.repository = repository;
            this.questionRepository = questionRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        // GET: /Answer/index
        public async Task<IActionResult> Index()
        {
            var answers = await repository.GetAllAsync();
            var model = mapper.Map<IEnumerable<AnswerViewModel>>(answers);
            return View(model);
        }
        // GET: Answer/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var answer = await repository.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }
        // GET: Answer/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var questions = await questionRepository.GetAllAsync();
            var users = await userRepository.GetAllAsync();

            ViewBag.Questions = new SelectList(questions, "Id", "Title");
            ViewBag.Users = new SelectList(users, "Id", "UserName");

            return View();
        }
        // POST: Answer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnswerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var questions = await questionRepository.GetAllAsync();
                var users = await userRepository.GetAllAsync();

                ViewBag.Questions = new SelectList(questions, "Id", "Title");
                ViewBag.Users = new SelectList(users, "Id", "UserName");

                return View(model);
            }

            var answer = mapper.Map<Answer>(model);
            await repository.AddAsync(answer);
            await repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Answer/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var answer = await repository.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            var model = mapper.Map<AnswerViewModel>(answer);
            var questions = await questionRepository.GetAllAsync();
            var users = await userRepository.GetAllAsync();
            ViewBag.Questions = new SelectList(questions, "Id", "Title");
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AnswerViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                var questions = await questionRepository.GetAllAsync();
                var users = await userRepository.GetAllAsync();
                ViewBag.Questions = new SelectList(questions, "Id", "Title", model.QuestionId);
                ViewBag.Users = new SelectList(users, "Id", "UserName", model.UserId);
                return View(model);
            }

            var answer = mapper.Map<Answer>(model);
            repository.Update(answer);
            await repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var answer = await repository.GetByIdAsync(id);
            if (answer == null)
                return NotFound();

            var model = mapper.Map<AnswerViewModel>(answer);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var answer = await repository.GetByIdAsync(id);
            if (answer == null)
                return NotFound();

            repository.Delete(answer);
            await repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




    }
}
