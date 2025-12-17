using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces.Courses;
using Core.Models.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.ViewModels.CoursesViewModels;

namespace Skillup_Academy.Controllers.Courses
{
	[Authorize(Roles = "Admin")] 
	public class CourseCategoriesController : Controller
    {
        
        ICourseCategoryRepsitory CourseCategoryRepsitory;
		private readonly SaveImage _settingsImages;
		private readonly IRepository<CourseCategory> _reposCourCategory;

		public CourseCategoriesController(ICourseCategoryRepsitory _CourseCategoryRepsitory,SaveImage settingsImages,IRepository<CourseCategory> reposCourCategory)
        {
            CourseCategoryRepsitory = _CourseCategoryRepsitory;
			_settingsImages = settingsImages;
			_reposCourCategory = reposCourCategory;
		}
        public IActionResult Index()
        {
            List<CourseCategory> CourseCategories = CourseCategoryRepsitory.GetAll();
            return View("Index", CourseCategories);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetByIdWithSubCategory(Guid Id)
        {
            var listSubCategory = _reposCourCategory.Query()
                .Include(s => s.SubCategories)
                .FirstOrDefault(i=>i.Id==Id); 

			return View(listSubCategory);
        }

        public IActionResult Create()
        {
            return View();
        }
         

        [HttpPost]
        public async Task<IActionResult> SaveCreate(CourseCategoryViewModel CCVM)
        {
            if (ModelState.IsValid)
            {
                CourseCategory courseCategory = new CourseCategory();
                courseCategory.Name = CCVM.Name;
                courseCategory.Description = CCVM.Description;
                courseCategory.IsActive = CCVM.IsActive;
                courseCategory.Icon =await _settingsImages.SaveImgAsync(CCVM.Image);
                CourseCategoryRepsitory.Add(courseCategory);
                CourseCategoryRepsitory.Save();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", CCVM);
        }

        // GET: CourseCategories/Edit/5
        public IActionResult Edit(Guid id)
        {
            CourseCategory courseCategory = CourseCategoryRepsitory.GetById(id);
            if (courseCategory == null)
            {
                return NotFound();
            }
			EditCourseCategoryViewModel CCVM = new EditCourseCategoryViewModel();
            CCVM.Id = id ;
            CCVM.Name = courseCategory.Name ;
            CCVM.Description = courseCategory.Description;
            CCVM.IsActive = courseCategory.IsActive;
            CCVM.ImageOld = courseCategory.Icon; 
            return View(CCVM);
        }

  
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditCourseCategoryViewModel CCVM)
        {
            if (id != CCVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                    CourseCategory OldCourseCategory = CourseCategoryRepsitory.GetById(id);
                    OldCourseCategory.Id = CCVM.Id;
                    OldCourseCategory.Name = CCVM.Name;
                    OldCourseCategory.Description = CCVM.Description;
                    OldCourseCategory.IsActive = CCVM.IsActive;
                    if (CCVM.Image != null)
                        OldCourseCategory.Icon = await _settingsImages.SaveImgAsync(CCVM.Image); 
                    CourseCategoryRepsitory.Update(OldCourseCategory);
                    CourseCategoryRepsitory.Save();
                    return RedirectToAction(nameof(Index));
            }
            return View("Edit", CCVM);
        }

        // GET: CourseCategories/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CourseCategory courseCategory = CourseCategoryRepsitory.GetById(id);

            if (courseCategory == null)
            {
                return NotFound();
            }

            return View(courseCategory);
        }

        // POST: CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            CourseCategory courseCategory = CourseCategoryRepsitory.GetById(id);
            if (courseCategory != null)
            {
                CourseCategoryRepsitory.Delete(courseCategory);
                CourseCategoryRepsitory.Save();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}
