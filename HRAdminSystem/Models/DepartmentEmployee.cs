namespace HRAdminSystem.Models
{
    public class DepartmentEmployee
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
