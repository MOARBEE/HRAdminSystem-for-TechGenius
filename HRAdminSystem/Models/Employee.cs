using HRAdminSystem.Enums;

namespace HRAdminSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }
        public Status Status { get; set; }
        public ICollection<Employee> ManagedEmployees { get; set; }
        public ICollection<DepartmentEmployee> DepartmentEmployees { get; set; }
    }
}
