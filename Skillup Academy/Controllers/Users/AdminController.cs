using AutoMapper;
using Core.Interfaces;
using Core.Interfaces;
using Core.Interfaces.Courses;
using Core.Interfaces.Reviews;
using Core.Interfaces.Subscriptions;
using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Lessons;
using Core.Models.Reviews;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Infrastructure.Repositories.Courses;
using Infrastructure.Repositories.Reviews;
using Infrastructure.Repositories.Subscriptions;
using Infrastructure.Repositories.Users;
using Infrastructure.Services.Lessons;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.ViewModels.AdminDashboard;
using Skillup_Academy.ViewModels.UsersViewModels;

namespace Skillup_Academy.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[action]")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository; 
        private readonly IRepository<Course> _repository;
        private readonly ICourseCategoryRepsitory _categoryRepository; 
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly SaveImage _saveImage;
        public AdminController(IStudentRepository studentRepository, ITeacherRepository teacherRepository
            , IRepository<Course> repository, ICourseCategoryRepsitory categoryRepository
            , ISubCategoryRepository subCategoryRepository, UserManager<User> userManager
            , SaveImage saveImage, SignInManager<User> signInManager)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _repository = repository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _userManager = userManager;
            _saveImage = saveImage;
            _signInManager = signInManager;
        }
        // /Admin/DashBoard
        public async Task<IActionResult> DashBoard()
        {
            int totalStudents = await _studentRepository.GetTotalStudentCountAsync();
            int totalTeachers = await _teacherRepository.GetTotalTeacherCountAsync();
            int totalCourses = await _repository.GetTotalCourseCountAsync();
            int totalCategories = await _categoryRepository.GetTotalCategoryCountAsync();
            int totalSubCategories = await _subCategoryRepository.GetTotalSubCategoryCountAsync();

            var viewModel = new DashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalActiveCourses = totalCourses,
                TotalTeachers = totalTeachers,
                TotalCategories = totalCategories,
                TotalSubCategories = totalSubCategories
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var viewModel = new AdminSettingViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CurrentProfilePicturePath = user.ProfilePicture,
                RegistrationDate = user.RegistrationDate,
                LastProfileUpdate = user.LastProfileUpdate
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Settings(AdminSettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userForDisplay = await _userManager.GetUserAsync(User);
                if (userForDisplay != null)
                {
                    model.RegistrationDate = userForDisplay.RegistrationDate;
                    model.LastProfileUpdate = userForDisplay.LastProfileUpdate;
                }
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.LastProfileUpdate = DateTime.Now;


            if (model.ProfilePicture != null)
            {
                user.ProfilePicture = await _saveImage.SaveImgAsync(model.ProfilePicture);
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user); 
                return RedirectToAction(nameof(DashBoard));
            }

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(Settings));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CourseDetails(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            bool canView = user.CanViewPaidCourses;

            var course = await _repository.Query()
                .Include(c => c.Category)
                .Include(l => l.Lessons)
                .Include(t => t.Teacher)
                .Include(s => s.SubCategory)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (course == null)
                return NotFound();


            if (course.IsFree || canView)
            {
                return View(course);
            }
            return RedirectToAction("ShowAllPlanInHome", "Subscription");
        }
    }
}
