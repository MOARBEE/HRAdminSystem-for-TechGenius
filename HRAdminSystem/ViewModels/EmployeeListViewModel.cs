using HRAdminSystem.Enums;

namespace HRAdminSystem.ViewModels
{
    public class EmployeeListViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string ManagerName { get; set; }
        public Status Status { get; set; }
        public List<string> Departments { get; set; }
    }
}
