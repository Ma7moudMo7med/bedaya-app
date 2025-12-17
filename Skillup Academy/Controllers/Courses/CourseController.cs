using AutoMapper;
using Core.Interfaces;
using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Enrollments;
using Core.Models.Reviews;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.ViewModels.CoursesViewModels;
using Skillup_Academy.ViewModels.SearchViewModels;

namespace Educational_Platform.Controllers.Courses
{
	[Authorize(Roles = "Admin,Instructor")]
	public class CourseController : Controller
    {
        private readonly IRepository<Course> _repository;
        private readonly ITeacherRepository _repoTeacher;
 		private readonly IRepository<SubCategory> _repoSubCategory;
		private readonly IRepository<SubscriptionPlan> _repoSubSubscriptionPlan;
		private readonly IRepository<CourseCategory> _repoCategory;
		private readonly IRepository<Student> _repoStudent;
        private readonly IRepository<Enrollment> _repoEnrollment;
        private readonly IRepository<CourseReview> _courseReviewRepository;
        private readonly IMapper _mapper;
		private readonly SaveImage _saveImage;
		private readonly UserManager<User> _userManager;
		private readonly DeleteImage _deleteImage;

		public CourseController(IRepository<Course> repository,IRepository<SubCategory> repoSubCategory,
 
            IRepository<CourseCategory> repoCategory, IMapper mapper,SaveImage saveImage,ITeacherRepository repoTeacher, IRepository<Student> repoStudent,
			UserManager<User> UserManager,DeleteImage deleteImage, IRepository<SubscriptionPlan> repoSubSubscriptionPlan
            , IRepository<Enrollment> repoEnrollment ,IRepository<CourseReview> courseReviewRepository)
 		{
			_repository = repository;
			_repoSubCategory = repoSubCategory;
			_repoCategory = repoCategory;
			_mapper = mapper;
			_saveImage = saveImage;
			_repoStudent = repoStudent;
			_userManager = UserManager;
			_deleteImage = deleteImage;
            _repoTeacher = repoTeacher;
			_repoSubSubscriptionPlan = repoSubSubscriptionPlan;
            _repoEnrollment = repoEnrollment;
            _courseReviewRepository = courseReviewRepository;
        }
 
        [AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> AllCourseInHeader( List<Guid>? categoryIds, List<Guid>? subcategoryIds, bool isfree, int page = 1)
		{
			int pageSize = 9;

			var results = _repository.Query()
			   .Include(c => c.Teacher)
			   .Include(c => c.SubCategory)
			   .Where(c => c.IsPublished)
			   .AsQueryable();
             
			if (categoryIds != null && categoryIds.Any()||categoryIds != null && categoryIds.Any())
			{
				results = results.Where(c =>
				   subcategoryIds.Contains(c.SubCategoryId ?? Guid.Empty) ||
				   categoryIds.Contains(c.CategoryId ?? Guid.Empty) || c.IsFree == isfree
			   );
			}

			int totalResults = await results.CountAsync();

			var pagedCourses = await results
			   .OrderByDescending(c => c.CreatedDate)  
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToListAsync();

			var viewModel = new SearchResultsViewModel
			{
				Courses = pagedCourses,
				TotalResults = totalResults,
				CurrentPage = page,
				PageSize = pageSize,
				SelectedCategoryIds = categoryIds,
				IsFree = isfree,
				Categories = await _repoCategory.GetAllAsync(),
				SubCategories = await _repoSubCategory.GetAllAsync()
			};

			return View(viewModel);
		}

 		[HttpGet]
		public async Task<IActionResult> ShowAll()
        {
            var currentUserId = _userManager.GetUserId(User);
            var allCourse = await _repository.Query()
                .ToListAsync();
            if (User.IsInRole("Instructor"))
            {
                var teacher = await _repoTeacher.GetTeacherAsync(currentUserId);
                if (teacher != null)
                {
                    allCourse = allCourse.Where(c => c.TeacherId == teacher.Id).ToList();
                }

            }
            return View(allCourse);
        }

		[AllowAnonymous]
		public async Task<IActionResult> AllCaurseIsPublished(int page = 1)
		{
			int pageSize = 9;  

			var allCourse = await _repository.Query()
				.Where(c => c.IsPublished)
				.OrderBy(c => c.Title)  
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync(); 
			 
			int totalCourses = await _repository.Query().Where(c => c.IsPublished).CountAsync();
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = (int)Math.Ceiling(totalCourses / (double)pageSize);

			return View(allCourse);
		}


		[HttpGet]
        public async Task<IActionResult> Create()
        {
			ViewBag.SubCategory= new SelectList(await _repoSubCategory.GetAllAsync(), "Id", "Name");
            ViewBag.Category= new SelectList(await _repoCategory.GetAllAsync(), "Id", "Name");
			   
			return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseViewModel VM)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(VM);

                course.CreatedDate = DateTime.Now;
                course.UpdatedDate = DateTime.Now;
                course.PreviewVideoUrl =await _saveImage.SaveImgAsync(VM.PreviewVideo);
                course.ThumbnailUrl = await _saveImage.SaveImgAsync(VM.ThumbnailUrl);
                if (course.IsPublished == true)
                    course.PublishedDate = DateTime.Now;

				var userIdString = _userManager.GetUserId(User);

                Guid teacherId;
                if (!Guid.TryParse(userIdString, out teacherId))
                {
                    teacherId = Guid.NewGuid();
                }
                course.TeacherId = teacherId;


                await _repository.AddAsync(course);
                await _repository.SaveChangesAsync();
                return RedirectToAction("ShowAll");
            }
             
			return View(VM);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

			ViewBag.SubCategory = new SelectList(await _repoSubCategory.GetAllAsync(), "Id", "Name");
			ViewBag.Category = new SelectList(await _repoCategory.GetAllAsync(), "Id", "Name");

			var courseVM = _mapper.Map<EditCourseViewModel>(course);
            return View(courseVM);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCourseViewModel newCourse)
        {
            if (ModelState.IsValid)
            {
                var oldCourse = await _repository.GetByIdAsync(newCourse.Id);
                if (oldCourse == null)
                {
                    return NotFound();
                }
                oldCourse.Title = newCourse.Title;
                oldCourse.Description = newCourse.Description;
                oldCourse.ShortDescription = newCourse.ShortDescription;
                 
				if (newCourse.ThumbnailUrlFile != null) 
                    oldCourse.ThumbnailUrl = await _saveImage.SaveImgAsync(newCourse.ThumbnailUrlFile);
				 
				if (newCourse.PreviewVideoFile != null) 
                    oldCourse.PreviewVideoUrl = await _saveImage.SaveImgAsync(newCourse.PreviewVideoFile);
				 
                oldCourse.IsFree = newCourse.IsFree;
                oldCourse.TotalLessons = newCourse.TotalLessons;
                oldCourse.TotalDuration = newCourse.TotalDuration;
                oldCourse.UpdatedDate = DateTime.Now;
                if (oldCourse.IsPublished == false && newCourse.IsPublished == true)
                {
                    oldCourse.PublishedDate = DateTime.Now;

                }
                oldCourse.IsPublished = newCourse.IsPublished;


                _repository.Update(oldCourse);
                await _repository.SaveChangesAsync();
                return RedirectToAction("ShowAll");
            }

			ViewBag.SubCategory = new SelectList(await _repoSubCategory.GetAllAsync(), "Id", "Name");
			ViewBag.Category = new SelectList(await _repoCategory.GetAllAsync(), "Id", "Name");
			return View(newCourse);
        }

        [AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> Details(Guid id)
		{
			var user = await _userManager.GetUserAsync(User); 
			if (user == null)
                return RedirectToAction("Login","Account");

			var student = await _repoStudent.GetByIdAsync(user.Id);
 			bool canView = user.CanViewPaidCourses;

            var subPlan = _repoSubSubscriptionPlan
                .Query().Where(i=>i.UserId==user.Id).FirstOrDefault();

			if (subPlan != null) 
			{
				if (student.TotalEnrollments == subPlan.MaxCourses)
				{
					user.CanViewPaidCourses = false;	
					await _userManager.UpdateAsync(user);
					await _repository.SaveChangesAsync();
					canView = false; 
				} 
			}
            var userId = _userManager.GetUserId(User);

            var enrollmentExists = await _repoEnrollment.Query()
                    .AnyAsync(e => e.CourseId == id && e.StudentId == Guid.Parse(userId));

            ViewBag.IsEnrolled = enrollmentExists;

            var userReview = await _courseReviewRepository.Query() 
                                     .FirstOrDefaultAsync(r => r.CourseId == id && r.UserId == Guid.Parse(userId));

            ViewBag.HasReviewed = (userReview != null); 

            ViewBag.ReviewId = userReview?.Id;
            //ViewBag.HasReviewed = reviewExists; 

            var course = await _repository.Query()
				.Include(c => c.Category)
				.Include(l => l.Lessons)
				.Include(t => t.Teacher)
				.Include(s => s.SubCategory)
				.FirstOrDefaultAsync(i => i.Id == id);

			if (course == null)
				return NotFound();
             
			if (course.IsFree || canView || User.IsInRole("Admin")||User.IsInRole("Instructor"))
			{
				return View(course);
			} 
            return RedirectToAction("ShowAllPlanInHome", "Subscription");
		}



		[HttpGet]
        public IActionResult Delete(Guid id)
        {
            var course = _repository.GetByIdAsync(id).Result;
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            _repository.Delete(course);
            await _repository.SaveChangesAsync();
            _deleteImage.DeleteImg(course.ThumbnailUrl);
            _deleteImage.DeleteImg(course.PreviewVideoUrl); 
			return RedirectToAction("ShowAll");
        }

        [HttpGet]
        public async Task<IActionResult> CourseShearch(string searchString,Guid teacherId)
        {
           var courses = await _repository.Query()
                .Where(c => c.TeacherId == teacherId &&
                       (c.Title.Contains(searchString) || c.Description.Contains(searchString)))
                .ToListAsync();
            return View("ShowAll", courses);
        }


    }
}
