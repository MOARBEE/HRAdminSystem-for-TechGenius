using HRAdminSystem.Data;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRAdminSystem.Pages.Employees
{
    [Authorize(Policy = "EmployeeOrManager")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public string DebugMessage { get; set; } = "Page accessed";

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            System.Diagnostics.Debug.WriteLine("Constructor called");
            _context = context;
            _userManager = userManager;
        }

        public IList<EmployeeListViewModel> Employees { get; set; }

        public async Task OnGetAsync()
        {
            System.Diagnostics.Debug.WriteLine($"User roles: {string.Join(", ", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))}");
            var currentUser = await _userManager.GetUserAsync(User);
            System.Diagnostics.Debug.WriteLine($"Current user: {currentUser?.Email}");
            var isHRAdmin = User.IsInRole("HRAdmin");

            var query = _context.Employees
                .Include(e => e.Manager)
                .Include(e => e.DepartmentEmployees)
                .ThenInclude(de => de.Department)
                .Select(e => new EmployeeListViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    EmailAddress = e.EmailAddress,
                    TelephoneNumber = e.TelephoneNumber,
                    ManagerName = e.Manager != null ? $"{e.Manager.FirstName} {e.Manager.LastName}" : "",
                    Status = e.Status,
                    Departments = e.DepartmentEmployees.Select(de => de.Department.Name).ToList()
                });

            if (!isHRAdmin)
            {
                query = query.Where(e => e.EmailAddress == currentUser.Email);
            }

            Employees = await query.ToListAsync();
        }
    }
}
