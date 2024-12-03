using HRAdminSystem.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRAdminSystem.ViewModels
{
    public class EmployeeFormViewModel
    {

        public EmployeeFormViewModel()
        {
            AvailableManagers = new List<SelectListItem>();
            AvailableDepartments = new List<SelectListItem>();
            SelectedDepartments = new List<int>();
        }
        public int? Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telephone Number")]
        public string TelephoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Departments")]
        public List<int> SelectedDepartments { get; set; } = new List<int>();

        public List<SelectListItem> AvailableManagers { get; set; }
        public List<SelectListItem> AvailableDepartments { get; set; }
    }
}
