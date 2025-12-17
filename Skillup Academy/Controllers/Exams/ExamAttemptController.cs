using Core.Interfaces.Exams;
using Core.Models.Courses;
using Core.Models.Exams;
using Infrastructure.Repositories.Exams;
using Infrastructure.Services.Exams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Skillup_Academy.ViewModels.ExamsViewModels;


namespace Skillup_Academy.Controllers.Exams
{
    public class ExamAttemptController : Controller
    {
        //_examAttemptRepository _examAttemptRepository = new _examAttemptRepository();
        IExamAttemptRepository _examAttemptRepository;
        public ExamAttemptController(IExamAttemptRepository examAttemptRepository)
        {
            _examAttemptRepository = examAttemptRepository;
        }
        // /ExamAttempt/ShowAll
        public IActionResult ShowAll()
        {
            List<ExamAttempt> ExamList = _examAttemptRepository.ShowAll();
            return View("ShowAll", ExamList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            ExamAttempt examAttempt = _examAttemptRepository.ShowDetails(id);
            return View("ShowDetails", examAttempt);
        }

        public IActionResult Create()
        {
            ExamAttemptViewModel EAVM = new ExamAttemptViewModel();
            EAVM.Exams = new SelectList(_examAttemptRepository.ShowAll(), "Id", "Name");
            EAVM.Students = new SelectList(_examAttemptRepository.ShowAll(), "Id", "FullName");
            return View("Create",EAVM);
        }

        public IActionResult SaveCreate(ExamAttemptViewModel exam)
        {
            if (ModelState.IsValid)
            {
                ExamAttempt examAttempt = new ExamAttempt();
                examAttempt.StartTime = exam.StartTime;
                examAttempt.EndTime = exam.EndTime;
                examAttempt.Score = exam.Score;
                examAttempt.TotalQuestions = exam.TotalQuestions;
                examAttempt.CorrectAnswers = exam.CorrectAnswers;
                examAttempt.IsPassed = exam.IsPassed;
                examAttempt.AttemptNumber = exam.AttemptNumber;
                examAttempt.ExamId = exam.ExamId;
                examAttempt.StudentId = exam.StudentId;
                _examAttemptRepository.ExamAttemptAdd(examAttempt);
                _examAttemptRepository.Save();
                return RedirectToAction("ShowAll");
            }
            exam.Exams = new SelectList(_examAttemptRepository.ShowAll(), "Id", "Name");
            exam.Students = new SelectList(_examAttemptRepository.ShowAll(), "Id", "FullName");
            return View(nameof(Create), exam);
        }

        public IActionResult Edit(Guid id)
        {
            ExamAttempt ExamEdit = _examAttemptRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", ExamEdit);
        }
        public IActionResult SaveEdit(ExamAttempt ExamAttemptSent, Guid id)
        {
            ExamAttempt OldExamAttempt = _examAttemptRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                OldExamAttempt.StartTime = ExamAttemptSent.StartTime;
                OldExamAttempt.EndTime = ExamAttemptSent.EndTime;
                OldExamAttempt.Score = ExamAttemptSent.Score;
                OldExamAttempt.TotalQuestions = ExamAttemptSent.TotalQuestions;
                OldExamAttempt.CorrectAnswers = ExamAttemptSent.CorrectAnswers;
                OldExamAttempt.IsPassed = ExamAttemptSent.IsPassed;
                OldExamAttempt.AttemptNumber = ExamAttemptSent.AttemptNumber;
                _examAttemptRepository.Update(OldExamAttempt);
                _examAttemptRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", ExamAttemptSent);
        }
        public IActionResult Delete(Guid id)
        {
            ExamAttempt ExamDelete = _examAttemptRepository.ShowDetails(id);
            return View("Delete", ExamDelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            ExamAttempt ExamDelete = _examAttemptRepository.ShowDetails(id);
            if (ExamDelete != null)
            {
                _examAttemptRepository.DeleteFromDB(ExamDelete);
                _examAttemptRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
