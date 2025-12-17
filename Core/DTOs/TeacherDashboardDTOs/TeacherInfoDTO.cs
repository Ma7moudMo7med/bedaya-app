using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs
{
    public class TeacherInfoDTO
    {
        public string FullName { get; set; }              // الاسم  
        public string Email { get; set; }                 // البريد الإلكتروني
        public string PhoneNumber { get; set; }           // رقم الهاتف
        public string Bio { get; set; }                    // السيرة الذاتية
        public string Qualifications { get; set; }         // المؤهلات العلمية
        public string Expertise { get; set; }              // التخصص
        public string ProfilePicture { get; set; }         // صورة البروفايل
    }
}
