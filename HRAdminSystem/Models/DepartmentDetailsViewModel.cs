using HRAdminSystem.Enums;

namespace HRAdminSystem.Models
{
    public class DepartmentDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public List<string> Employees { get; set; } = new List<string>();
    }
}
