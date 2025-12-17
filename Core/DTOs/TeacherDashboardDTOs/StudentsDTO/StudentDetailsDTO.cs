using Core.Enums;
using Core.Models.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs.StudentsDTO
{
    public class StudentDetailsDTO
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public int CoursesCount { get; set; }
        public StudentStatus Status { get; set; } // Active / Inactive

        public ICollection<StudentCourseDTO> Courses { get; set; } 
    }
}
