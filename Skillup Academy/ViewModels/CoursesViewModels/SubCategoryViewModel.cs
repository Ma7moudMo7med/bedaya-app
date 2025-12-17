using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.CoursesViewModels
{
    public class SubCategoryViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Course Name")]
        [MaxLength(20, ErrorMessage = "Less than 20")]
        [MinLength(3, ErrorMessage = "more than 3")]
        [Required(ErrorMessage = "Required Feild")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Contain Only Letters and Spaces")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Less than 20")]
        [MinLength(20, ErrorMessage = "more than 20")]
        [Required(ErrorMessage = "Required Feild")]

        public string Description { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Required Feild")]
        public Guid CategoryId { get; set; }              

        public bool IsActive { get; set; } = true;
        [ValidateNever]
        public SelectList Categories { get; set; }
    }
}
