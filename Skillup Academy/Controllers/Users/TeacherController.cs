using Core.Interfaces;
using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Helper;
using Skillup_Academy.ViewModels.TeacherDashboard;
using Skillup_Academy.ViewModels.UsersViewModels;
using System.Security.Claims;

namespace Skillup_Academy.Controllers.Users
{
    //[Authorize]
    [Authorize(Roles = "Instructor,Admin")]
    [Route("Teacher/[action]")]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<Course> _repository;
        //------- for Admin Dashboard
        private readonly SaveImage _saveImage;
        private readonly FileService _fileService;
        public TeacherController(ITeacherRepository teacherRepository, UserManager<User> userManager, SaveImage saveImage
                                , FileService fileService, SignInManager<User> signInManager, IRepository<Course> repository)
        {
            _teacherRepository = teacherRepository;
            _userManager = userManager;
            _saveImage = saveImage;
            _fileService = fileService;
            _signInManager = signInManager;
            _repository = repository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            //var teacher = await _userManager.FindByIdAsync(userId);
            //if (teacher == null)
            //    return NotFound("Teacher not found.");
            //var teacherGuid = Guid.Parse(user.Id.ToString());

            var dashboardData = await _teacherRepository.GetTeacherDashboardAsync(user.Id);
            if (dashboardData == null)
                return NotFound("Dashboard data not available.");

            var coursesList = dashboardData.Courses
             .Select(c => new CourseDashboardViewModel
             {
                 CourseId = c.CourseId,
                 Title = c.Title,
                 IsPublished = c.IsPublished,
                 TotalDuration = c.TotalDuration,
                 ThumbnailUrl = c.ThumbnailUrl,
                 AverageRating = c.AverageRating,
                 CreatedDate = c.CreatedDate,
                 TotalLessons = c.TotalLessons,
                 TotalStudents = c.TotalStudents,
             })
             .ToList();

            var viewModel = new TeacherDashboardVM
            {
                TeacherName = user.FullName ?? string.Empty,
                TotalCourses = dashboardData.TotalCourses,
                TotalStudents = dashboardData.TotalStudents,
                ProfilePictureUrl = dashboardData.ProfilePictureUrl,
                TeacherBio = dashboardData.TeacherBio,
                TeacherExpertise = dashboardData.TeacherExpertise,
                TeacherRating = dashboardData.TeacherRating,
                Courses = coursesList
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpGet]
         public async Task<IActionResult> MyCourses()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Unauthorized();

                var teacher = await _teacherRepository.GetTeacherAsync(userId);
                if (teacher == null)
                    return NotFound("Teacher not found.");

                if (!Guid.TryParse(teacher.Id.ToString(), out var teacherGuid))
                    return BadRequest("Invalid teacher ID format.");

                var courses = await _teacherRepository.GetTeacherCoursesAsync(teacherGuid);
                var viewModel = courses.Select(c => new CourseDashboardViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    IsPublished = c.IsPublished,
                    ThumbnailUrl = c.ThumbnailUrl,
                    IsFree = c.IsFree,
                    AverageRating = c.AverageRating,
                    TotalDuration = c.TotalDuration,
                    CreatedDate = c.CreatedDate,
                    TotalLessons = c.TotalLessons,
                    TotalStudents = c.TotalStudents,
                    teacherId = c.teacherId

                }).ToList();
                ViewBag.TeacherName = teacher.FullName;
                ViewBag.ProfilePictureUrl = teacher.ProfilePicture;
                ViewBag.TeacherExpertise = teacher.Expertise;
                ViewBag.TeacherId = teacher.Id;
                return View(viewModel);
            }
        
        public async Task<IActionResult> CourseSearch(string searchString, Guid teacherId)
  {
            // هجيب كل كورسات المدرّس
            var allCourses = await _teacherRepository.GetTeacherCoursesAsync(teacherId);

            // فلترة لو فيه بحث
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();

                allCourses = allCourses
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.Title) && c.Title.ToLower().Contains(searchString)) ||
                        (!string.IsNullOrEmpty(c.Description) && c.Description.ToLower().Contains(searchString))
                    )
                    .ToList();
            }

            // لازم تحوّل للـ ViewModel زي MyCourses
            var viewModel = allCourses.Select(c => new CourseDashboardViewModel
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description,
                IsPublished = c.IsPublished,
                ThumbnailUrl = c.ThumbnailUrl,
                IsFree = c.IsFree,
                AverageRating = c.AverageRating,
                TotalDuration = c.TotalDuration,
                CreatedDate = c.CreatedDate,
                TotalLessons = c.TotalLessons,
                TotalStudents = c.TotalStudents,
                teacherId = c.teacherId
            }).ToList();

            return View("MyCourses", viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Setting()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            //var teacherGuid = Guid.Parse(userId);

           var teacherInfo = await _teacherRepository.GetTeacherInfoAsync(user.Id);
            if (teacherInfo == null)
                return NotFound("Teacher info not available.");

            var viewModel = new TeacherInfoVM
            {
                FullName = teacherInfo.FullName,
                Email = teacherInfo.Email,
                PhoneNumber = teacherInfo.PhoneNumber,
                Bio = teacherInfo.Bio,
                Qualifications = teacherInfo.Qualifications,
                Expertise = teacherInfo.Expertise,
                ProfilePicture = teacherInfo.ProfilePicture
            };

            return View(viewModel);        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setting(TeacherInfoVM model)
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();

                // Debug in console
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var teacherInfo = await _teacherRepository.GetTeacherInfoAsync(user.Id);
            if(teacherInfo == null) 
                return NotFound("Teacher info not available.");
            // Update teacher info
            teacherInfo.FullName = model.FullName;
            teacherInfo.Email = model.Email;
            teacherInfo.PhoneNumber = model.PhoneNumber;
            teacherInfo.Bio = model.Bio;
            teacherInfo.Qualifications = model.Qualifications;
            teacherInfo.Expertise = model.Expertise;
            // Update profile picture if provided
            if (model.ProfilePictureFile != null)
            {
                // مكان حفظ الصور
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/teachers");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}_{model.ProfilePictureFile.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePictureFile.CopyToAsync(stream);
                }

                // عدل الرابط في DB
                teacherInfo.ProfilePicture = $"/images/teachers/{fileName}";
            }
            else if (!string.IsNullOrEmpty(model.ProfilePicture))
            {
                // لو ما غيرش المستخدم الصورة، احتفظ بالقيمة القديمة
                teacherInfo.ProfilePicture = model.ProfilePicture;
            }
            // Save changes
            var result = await _teacherRepository.UpdateTeacherInfoAsync(teacherInfo,user.Id);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Settings updated successfully!";
                return RedirectToAction("Setting");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update settings.");
                return View(model);
            }

        }


        [HttpGet]
        public async Task<IActionResult> Students()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var teacher = await _teacherRepository.GetTeacherAsync(user.Id.ToString());
            if (!Guid.TryParse(teacher.Id.ToString(), out var teacherGuid))
                return BadRequest("Invalid teacher ID format.");
             var totalStudents = await _teacherRepository.GetTotalStudentsAsync(teacherGuid);

             var activeStudents = await _teacherRepository.GetActiveStudentsAsync(teacherGuid);

             var completeStudents = await _teacherRepository.GetCompleteStudentsAsync(teacherGuid);

             var totalCourses = await _teacherRepository.GetTeacherCoursesAsync(teacherGuid);

             var studentVM = new StudentListViewModel
            {
                TotalStudents = totalStudents,
                ActiveStudents = activeStudents,
                CompletedStudents = completeStudents,
                TeacherCourses = totalCourses.Select(c => new Core.DTOs.TeacherDashboardDTOs.CourseDashboardDTO
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    IsPublished = c.IsPublished,
                    CreatedDate = c.CreatedDate,
                    TotalLessons = c.TotalLessons,
                    TotalStudents = c.TotalStudents
                }).ToList()
            };
            var studentsList = await _teacherRepository.GetStudentsAsync(teacherGuid, 1, 20, null, null, null);
               studentVM.Students = studentsList.Students.Select(s => new StudentVM
                {
                  StudentId = s.StudentId,
                  FullName = s.FullName,
                  Email = s.Email,
                  CoursesCount = s.CoursesCount,
                 Status = s.Status
                }).ToList();
            studentVM.PageNumber = studentsList.PageNumber;
            studentVM.PageSize = studentsList.PageSize;
            studentVM.TotalRecords = studentsList.TotalRecords;
            return View(studentVM);
        }
        public async Task<IActionResult> StudentDetails(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var teacher = await _teacherRepository.GetTeacherAsync(user.Id.ToString());
            var studentDetails = await _teacherRepository.GetStudentDetailsAsync(user.Id, id);
            if (studentDetails == null)
                return NotFound("Student details not available.");
            var viewModel = new StudentDetailsVM
            {
                //StudentId = studentDetails.StudentId,
                FullName = studentDetails.FullName,
                Email = studentDetails.Email,
                PhoneNumber = studentDetails.PhoneNumber,
                ProfilePicture = studentDetails.ProfilePicture,
                CoursesCount = studentDetails.CoursesCount,
                Status = studentDetails.Status,
                Courses = studentDetails.Courses.Select(c => new Core.DTOs.TeacherDashboardDTOs.StudentsDTO.StudentCourseDTO
                {
                    CourseId = c.CourseId,
                    CourseTitle = c.CourseTitle,
                    CourseDescription = c.CourseDescription,
                    CourseImage = c.CourseImage,
                    EnrolledAt = c.EnrolledAt,
                    Status = c.Status
                }).ToList()
            };
            ViewBag.TeacherName = teacher.FullName;
            ViewBag.ProfilePictureUrl = teacher.ProfilePicture;
            ViewBag.TeacherExpertise = teacher.Expertise;
            ViewBag.TeacherId = teacher.Id;
            return View(viewModel);
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




        

        //////////////////////////////////////////////////////////// For Admin Dashboard////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Teacher> Teachers = await _teacherRepository.GetAll();
            return View("Index", Teachers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TeacherViewModel
            {
                IsActive = true
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            var file = _fileService.GetDefaultAvatar();

            if (ModelState.IsValid)
            {
                var teacher = new Teacher
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

                var result = await _teacherRepository.CreateTeacherAsync(teacher, model.Password);

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
            if (string.IsNullOrEmpty(id)) return NotFound();
            if (!Guid.TryParse(id, out var teacherGuid))
            {
                return BadRequest("Invalid teacher ID format.");
            }
            var teacherEntity = await _teacherRepository.GetTeacherAsync(id);
            var courses = await _teacherRepository.GetTeacherCoursesAsync(teacherGuid);

            if (teacherEntity == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacherEntity.Id.ToString(),
                UserName = teacherEntity.UserName,
                Email = teacherEntity.Email,
                FullName = teacherEntity.FullName,
                PhoneNumber = teacherEntity.PhoneNumber,
                RegistrationDate = teacherEntity.RegistrationDate,
                LastLoginDate = teacherEntity.LastLoginDate,
                IsActive = teacherEntity.IsActive,
                TotalCourses = courses.Count
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var teacherToEdit = await _teacherRepository.GetTeacherAsync(id);

            if (teacherToEdit == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacherToEdit.Id.ToString(),
                Email = teacherToEdit.Email,
                UserName = teacherToEdit.UserName,
                FullName = teacherToEdit.FullName,
                IsActive = teacherToEdit.IsActive,
                RegistrationDate = teacherToEdit.RegistrationDate,
                LastLoginDate = teacherToEdit.LastLoginDate,
                PhoneNumber = teacherToEdit.PhoneNumber,
                TotalCourses = teacherToEdit.TotalCourses
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Teacher model)
        {
            var teacherToUpdate = await _teacherRepository.GetTeacherAsync(id);
            if (teacherToUpdate == null) return NotFound();

            teacherToUpdate.Email = model.Email;
            teacherToUpdate.UserName = model.UserName;
            teacherToUpdate.FullName = model.FullName;
            teacherToUpdate.IsActive = model.IsActive;
            teacherToUpdate.RegistrationDate = model.RegistrationDate;
            teacherToUpdate.LastLoginDate = model.LastLoginDate;
            teacherToUpdate.PhoneNumber = model.PhoneNumber;
            teacherToUpdate.TotalCourses = model.TotalCourses;

            var result = await _teacherRepository.UpdateTeacherAsync(teacherToUpdate);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var teacherEntity = await _teacherRepository.GetTeacherAsync(id);

            if (teacherEntity == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacherEntity.Id.ToString(),
                UserName = teacherEntity.UserName,
                Email = teacherEntity.Email,
                FullName = teacherEntity.FullName,
                PhoneNumber = teacherEntity.PhoneNumber,
                RegistrationDate = teacherEntity.RegistrationDate,
                LastLoginDate = teacherEntity.LastLoginDate,
                IsActive = teacherEntity.IsActive,
                TotalCourses = teacherEntity.TotalCourses
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userToDelete = await _teacherRepository.GetTeacherAsync(id);

            if (userToDelete != null)
            {
                var result = await _teacherRepository.DeleteTeacherAsync(userToDelete);

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
