using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs.StudentsDTO
{
    public class StudentCourseDTO
    {
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImage { get; set; }
        public DateTime EnrolledAt { get; set; }
        public StudentStatus Status { get; set; } // Active / Completed / Dropped
    }
}
