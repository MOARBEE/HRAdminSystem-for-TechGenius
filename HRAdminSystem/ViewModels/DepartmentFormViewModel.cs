using HRAdminSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRAdminSystem.ViewModels
{
    public class DepartmentFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

    }
}
