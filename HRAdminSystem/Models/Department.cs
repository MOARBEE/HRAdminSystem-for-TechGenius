using HRAdminSystem.Enums;

namespace HRAdminSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public ICollection<DepartmentEmployee> DepartmentEmployees { get; set; }
    }
}
