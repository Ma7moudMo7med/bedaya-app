using Core.Models.Courses;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Courses
{
    public class CourseBL
    {

        AppDbContext Context = new AppDbContext();

        public List<Course> GetAll()
        {
            // تم تغيير الأقواس المختصرة
            return Context.Courses.ToList();
        }

        public Course GetById(Guid id)
        {
            // تم تغيير الأقواس المختصرة
            return Context.Courses.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Course course)
        {
            Context.Add(course);
            Context.SaveChanges();
        }

        public void Update()
        {
            Context.SaveChanges();
        }

        public void Delete(Course course)
        {
            Context.Courses.Remove(course);
            Context.SaveChanges();
        }

        public bool CourseExists(Guid id)
        {
            // تم تغيير الأقواس المختصرة
            return Context.Courses.Any(e => e.Id == id);
        }
    }
}