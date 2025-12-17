using Core.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Courses
{
    public interface ICourseCategoryRepsitory
    {
        public List<CourseCategory> GetAll();
        public void Add(CourseCategory TaskItem);
        public CourseCategory GetById(Guid Id);
        public void Delete(CourseCategory TaskItem);

        public void Update(CourseCategory TaskItem);
        public void Save();
        Task<int> GetTotalCategoryCountAsync();

    }
}
