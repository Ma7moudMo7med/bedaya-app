using Core.Models.Courses;
using Core.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Reviews
{
    public interface ICourseReviewRepository
    {
        public List<CourseReview> GetAll(Guid id);
        public void Add(CourseReview CourseReview);
        public CourseReview GetById(Guid Id);
        public void Delete(CourseReview CourseReview);

        public void Update(CourseReview CourseReview);
        public void Save();
    }
}
