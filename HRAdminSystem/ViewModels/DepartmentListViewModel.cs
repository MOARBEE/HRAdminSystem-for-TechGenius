using HRAdminSystem.Enums;

namespace HRAdminSystem.ViewModels
{
    public class DepartmentListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public int EmployeeCount { get; set; }
    }
}
