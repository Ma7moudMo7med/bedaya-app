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
    public class SubCategoryRepository : ISubCategoryRepository
    {
        //AppDbContext Context = new AppDbContext();
        AppDbContext Context;
        public SubCategoryRepository(AppDbContext _Context)
        {
            Context = _Context;
        }
        public List<SubCategory> GetAll()
        {
            return Context.SubCategories.Include(c=>c.Category).ToList();
        }
        public void Add(SubCategory CourseCategory)
        {
            Context.Add(CourseCategory);
        }
        public SubCategory GetById(Guid Id)
        {
            return Context.SubCategories.FirstOrDefault(c => c.Id == Id);
        }
        public void Update(SubCategory SubCategory)
        {
            Context.Update(SubCategory);
        }
        public void Delete(SubCategory SubCategory)
        {
            Context.SubCategories.Remove(SubCategory);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        public async Task<int> GetTotalSubCategoryCountAsync()
        {
            return await Context.SubCategories.CountAsync();
        }
    }
}
