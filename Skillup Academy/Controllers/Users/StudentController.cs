using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Helper;
using Skillup_Academy.ViewModels.StudentsViewModels;
using Skillup_Academy.ViewModels.UsersViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Skillup_Academy.Controllers.Users
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly SaveImage _saveImage;
        private readonly FileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public StudentController(IStudentRepository studentRepository, SaveImage saveImage, FileService fileService, UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _studentRepository = studentRepository;
            _saveImage = saveImage;
            _fileService = fileService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //DashBoard for student
        //public async Task<IActionResult> DashBoard()
        //{
        //    var viewmodel = new StudentDashboardViewModel();
        //    return View("DashBoard", viewmodel);
        //}
        //Profile 
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Take user
            var user = await _userManager.FindByIdAsync(userId);

            var student = user as Student;
            if (student == null)
                return BadRequest("Current user is not a student.");

            // الصورة
            var profileImage = string.IsNullOrEmpty(student.ProfilePicture)
                                    ? Url.Content("/img/Profile/download.png")
                                    : Url.Content("~/img/Profile/" + student.ProfilePicture);


            var vm = new StudentProfileViewModel
            {
                Id = student.Id,
                FullName = student.FullName,
                UserName = student.UserName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                ProfilePicture = profileImage,

                RegistrationDate = student.RegistrationDate,
                LastLoginDate = student.LastLoginDate,
                LastProfileUpdate = student.LastProfileUpdate,

                IsActive = student.IsActive,
                CanViewPaidCourses = student.CanViewPaidCourses,

                CompletedCourses = student.CompletedCourses,
                TotalEnrollments = student.TotalEnrollments
            };

            return View("Profile",vm);
        }

        //Edit 
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = user as Student;

            if (student == null)
                return BadRequest("Current user is not a student.");

            var vm = new StudentEditProfileViewModel
            {
                Id = student.Id,
                FullName = student.FullName,
                UserName = student.UserName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                CurrentProfilePicture = student.ProfilePicture
            };

            return View(vm);
        }

        //SaveEdit
        [HttpPost]
        public async Task<IActionResult> SaveEditProfile(StudentEditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("EditProfile", model);

            var user = await _userManager.GetUserAsync(User);
            var student = user as Student;

            if (student == null)
                return BadRequest("User is not a student.");

            bool hasChanges = false;

            // Full Name
            if (student.FullName != model.FullName)
            {
                student.FullName = model.FullName;
                hasChanges = true;
            }

            // Username
            if (student.UserName != model.UserName)
            {
                var usernameResult = await _userManager.SetUserNameAsync(student, model.UserName);
                if (!usernameResult.Succeeded)
                {
                    foreach (var error in usernameResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View("EditProfile", model);
                }
                hasChanges = true;
            }

            // Email
            if (student.Email != model.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(student, model.Email);
                if (!emailResult.Succeeded)
                {
                    foreach (var error in emailResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View("EditProfile", model);
                }
                hasChanges = true;
            }

            // Phone Number
            if (student.PhoneNumber != model.PhoneNumber)
            {
                student.PhoneNumber = model.PhoneNumber;
                hasChanges = true;
            }

            // Profile Picture
            if (model.NewProfilePicture != null)
            {
                var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.NewProfilePicture.FileName)}";
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Profile", newFileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await model.NewProfilePicture.CopyToAsync(stream);
                }

                // حذف الصورة القديمة لو موجودة
                if (!string.IsNullOrEmpty(student.ProfilePicture))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Profile", student.ProfilePicture);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                student.ProfilePicture = newFileName;
                hasChanges = true;
            }

            // Save Changes
            if (hasChanges)
            {
                student.LastProfileUpdate = DateTime.Now;
                await _userManager.UpdateAsync(student);
            }

            //TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        //password view
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        //savechangepassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(StudentChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // تغيير الباسورد
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            // بعد التغيير لازم نعمل Refresh للسيشن عشان يفضل لوجين
            await _signInManager.RefreshSignInAsync(user);

            //TempData["Success"] = "Password updated successfully!";
            return RedirectToAction("Profile");
        }

        //My Course
        public async Task<IActionResult> MyCourse()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var student = await _userManager.FindByIdAsync(userId) as Student;
            if (student == null) return Unauthorized();

            var enrollments = await _studentRepository.GetStudentEnrollments(student.Id);

            return View("MyCourse",enrollments);
        }


        //My Subscription
        public async Task<IActionResult> MySubscription()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var student = await _userManager.FindByIdAsync(userId) as Student;
            if (student == null)
                return Unauthorized();

            var subscription = await _studentRepository.GetStudentActiveSubscriptionAsync(student.Id);

            if (subscription == null)
            {
                return View("MySubscription", null); // لسه مشتركش
            }

            var vm = new StudentSubscriptionViewModel
            {
                SubscriptionName = subscription.Subscription?.Name,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive,
                MaxCourses = subscription.MaxCourses,
                PaidAmount = subscription.PaidAmount,
                TransactionId = subscription.TransactionId
            };

            return View("MySubscription", vm);
        }


        public async Task<IActionResult> Index()
        {
            List<Student> Students = await _studentRepository.GetAll();
            return View("Index", Students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new StudentViewModel
            {
                IsActive = true 
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            var file = _fileService.GetDefaultAvatar();
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    RegistrationDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = model.IsActive,
                    ProfilePicture = model.ClientFile != null
                              ? await _saveImage.SaveImgAsync(model.ClientFile)
                              : await _saveImage.SaveImgAsync(file)
                };

                var result = await _studentRepository.CreateStudentAsync(student, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var studentEntity = await _studentRepository.GetStudentByIdAsync(id);

            if (studentEntity == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = studentEntity.Id.ToString(), 
                UserName = studentEntity.UserName,
                Email = studentEntity.Email,
                FullName = studentEntity.FullName,
                PhoneNumber = studentEntity.PhoneNumber,
                RegistrationDate = studentEntity.RegistrationDate,
                LastLoginDate = studentEntity.LastLoginDate,
                IsActive = studentEntity.IsActive,
                TotalEnrollments = studentEntity.TotalEnrollments,
                CompletedCourses = studentEntity.CompletedCourses
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var studentToEdit = await _studentRepository.GetStudentByIdAsync(id);

            if (studentToEdit == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = studentToEdit.Id.ToString(), // ضروري تحويل الـ Guid إلى string
                Email = studentToEdit.Email,
                UserName = studentToEdit.UserName,
                FullName = studentToEdit.FullName,
                IsActive = studentToEdit.IsActive,
                RegistrationDate = studentToEdit.RegistrationDate,
                LastLoginDate = studentToEdit.LastLoginDate,
                PhoneNumber = studentToEdit.PhoneNumber,
                TotalEnrollments = studentToEdit.TotalEnrollments,
                CompletedCourses = studentToEdit.CompletedCourses
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, Student model)
        {
            var studentToUpdate = await _studentRepository.GetStudentByIdAsync(id);
            if (studentToUpdate == null) return NotFound();

            studentToUpdate.Email = model.Email;
            studentToUpdate.UserName = model.UserName;
            studentToUpdate.FullName = model.FullName;
            studentToUpdate.IsActive = model.IsActive;
            studentToUpdate.RegistrationDate = model.RegistrationDate;
            studentToUpdate.LastLoginDate = model.LastLoginDate;
            studentToUpdate.PhoneNumber = model.PhoneNumber;
            studentToUpdate.TotalEnrollments = model.TotalEnrollments;
            studentToUpdate.CompletedCourses = model.CompletedCourses;

            var result = await _studentRepository.UpdateStudentAsync(studentToUpdate);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var studentEntity = await _studentRepository.GetStudentByIdAsync(id);

            if (studentEntity == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = studentEntity.Id.ToString(),
                UserName = studentEntity.UserName,
                Email = studentEntity.Email,
                FullName = studentEntity.FullName,
                PhoneNumber = studentEntity.PhoneNumber,
                RegistrationDate = studentEntity.RegistrationDate,
                LastLoginDate = studentEntity.LastLoginDate,
                IsActive = studentEntity.IsActive,
                TotalEnrollments = studentEntity.TotalEnrollments,
                CompletedCourses = studentEntity.CompletedCourses
            };

            return View(viewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userToDelete = await _studentRepository.GetStudentByIdAsync(id);

            if (userToDelete != null)
            {
                var result = await _studentRepository.DeleteStudentAsync(userToDelete);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
