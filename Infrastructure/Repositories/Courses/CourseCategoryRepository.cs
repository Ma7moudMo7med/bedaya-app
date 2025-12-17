using Core.Interfaces.Courses;
using Core.Models.Courses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Courses
{
    public class CourseCategoryRepository : ICourseCategoryRepsitory
    {
        //AppDbContext Context = new AppDbContext();
        AppDbContext Context;
        public CourseCategoryRepository(AppDbContext _Context)
        {
            Context = _Context;
        }
        public List<CourseCategory> GetAll()
        {
            return Context.CourseCategories.ToList();
        }
        public void Add(CourseCategory CourseCategory)
        {
            Context.Add(CourseCategory);
        }
        public CourseCategory GetById(Guid Id)
        {
            return Context.CourseCategories.FirstOrDefault(c => c.Id == Id);
        }
        public void Update(CourseCategory CourseCategory)
        {
            Context.Update(CourseCategory);
        }
        public void Delete(CourseCategory cource)
        {
            Context.CourseCategories.Remove(cource);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        public async Task<int> GetTotalCategoryCountAsync()
        {
            return await Context.CourseCategories.CountAsync();
        }

    }
}
