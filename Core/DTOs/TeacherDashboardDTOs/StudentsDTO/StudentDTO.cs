using Core.Enums;
using Core.Models.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs.StudentsDTO
{
    public class StudentDTO
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Course info for list view
        public string CourseTitle { get; set; }
        public int CoursesCount { get; set; }
        public StudentStatus Status { get; set; }
        //public string CourseProgress { get; set; }


    }
}
