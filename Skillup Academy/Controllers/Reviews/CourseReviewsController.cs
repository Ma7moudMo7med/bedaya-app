using Core.Interfaces;
using Core.Interfaces.Courses;
using Core.Interfaces.Reviews;
using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Reviews;
using Core.Models.Users;
using Infrastructure.Data;
using Infrastructure.Services.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.ViewModels.CourseReviewViewModel;
using Skillup_Academy.ViewModels.CoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skillup_Academy.Controllers.Reviews
{
    public class CourseReviewsController : Controller
    {
        AppDbContext Context;
        private readonly ICourseReviewRepository CourseReviewRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRepository<Course> _repository;
        private readonly UserManager<User> _userManager;

        public CourseReviewsController(ICourseReviewRepository _CourseReviewRepository, AppDbContext _Context,
                                    IStudentRepository studentRepository, IRepository<Course> repository,
                                    IStudentRepository repoStudent, UserManager<User> userManager)
        {
            //Context = _Context;
            CourseReviewRepository = _CourseReviewRepository;
            _studentRepository = studentRepository;
            _repository = repository;
            _userManager = userManager;
        }
        // GET: CourseReviews/index
        public IActionResult Index(Guid id)
        {
            List<CourseReview> CourseReviews = CourseReviewRepository.GetAll(id);
            ViewBag.CourseId = id;
            return View("Index", CourseReviews);
        }

        

        // GET: CourseReviews/Create
        public async Task<IActionResult> Create(Guid courseId)
        {
            CourseReviewViewModel CRVM = new CourseReviewViewModel();
            CRVM.CourseId = courseId;
            return View("Create", CRVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Guid courseId, CourseReviewViewModel CRVM)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                CourseReview CourseReview = new CourseReview();
                CourseReview.Comment = CRVM.Comment;
                CourseReview.ReviewDate = DateTime.Now;
                CourseReview.Id = Guid.NewGuid();
                CourseReview.ContentRating = CRVM.ContentRating;
                CourseReview.TeachingRating = CRVM.TeachingRating;
                CourseReview.IsApproved = CRVM.IsApproved;
                CourseReview.CourseId = CRVM.CourseId;
                CourseReview.UserId = Guid.Parse(userId);
                CourseReviewRepository.Add(CourseReview);
                CourseReviewRepository.Save();
                return RedirectToAction(nameof(Index), new { id = CRVM.CourseId });
            }
            return View("Create", CRVM);
        }

        // GET: CourseReviews/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            CourseReview CourseReview = CourseReviewRepository.GetById(id);
            if (CourseReview == null)
            {
                return NotFound(); 
            }
            CourseReviewViewModel CRVM = new CourseReviewViewModel();
            CRVM.Rating = CourseReview.Rating;
            CRVM.Comment = CourseReview.Comment;
            CRVM.ReviewDate = DateTime.Now;
            CRVM.Id = CourseReview.Id;
            CRVM.ContentRating = CourseReview.ContentRating;
            CRVM.TeachingRating = CourseReview.TeachingRating;
            CRVM.IsApproved = CourseReview.IsApproved;
            CRVM.CourseId = CourseReview.CourseId;
            if (CourseReview == null)
            {
                return NotFound();
            }
            return View("Edit", CRVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CourseReviewViewModel CRVM)
        {
            if (id != CRVM.Id)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                    CourseReview OldCourseReview = CourseReviewRepository.GetById(id);
                    OldCourseReview.Comment = CRVM.Comment;
                    OldCourseReview.ReviewDate = DateTime.Now;
                    OldCourseReview.ContentRating = CRVM.ContentRating;
                    OldCourseReview.TeachingRating = CRVM.TeachingRating;
                    OldCourseReview.IsApproved = CRVM.IsApproved;
                    OldCourseReview.CourseId = CRVM.CourseId;
                    CourseReviewRepository.Update(OldCourseReview);
                    CourseReviewRepository.Save();

                return RedirectToAction(nameof(Index), new { id = CRVM.CourseId });
            }
            return View("Edit", CRVM);
        }

        // GET: CourseReviews/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CourseReview CourseReview = CourseReviewRepository.GetById(id);

            if (CourseReview == null)
            {
                return NotFound();
            }

            return View("Delete",CourseReview);
        }

        // POST: CourseReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            CourseReview CourseReview = CourseReviewRepository.GetById(id);
            if (CourseReview != null)
            {
                CourseReviewRepository.Delete(CourseReview);
                CourseReviewRepository.Save();
                return RedirectToAction(nameof(Index), new { id = CourseReview.CourseId });
            }
            return NotFound();
        }

    }
}
