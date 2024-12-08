using HRAdminSystem.Data;
using HRAdminSystem.ViewModels;
using HRAdminSystem.Enums;
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

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? DepartmentId { get; set; }
        [BindProperty(SupportsGet = true)]
        public HRAdminSystem.Enums.Status? StatusFilter { get; set; }

        public List<DepartmentListViewModel> DepartmentsList { get; set; }
        public IList<EmployeeListViewModel> Employees { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            System.Diagnostics.Debug.WriteLine("Constructor called");
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            System.Diagnostics.Debug.WriteLine($"User roles: {string.Join(", ", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))}");
            var currentUser = await _userManager.GetUserAsync(User);
            System.Diagnostics.Debug.WriteLine($"Current user: {currentUser?.Email}");
            var isHRAdmin = User.IsInRole("HRAdmin");

            
            DepartmentsList = await _context.Departments
                .Select(d => new DepartmentListViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();

            var query = _context.Employees
                .Include(e => e.Manager)
                .Include(e => e.DepartmentEmployees)
                .ThenInclude(de => de.Department)
                .AsQueryable();

            
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(SearchTerm) ||
                    e.LastName.Contains(SearchTerm) ||
                    e.EmailAddress.Contains(SearchTerm));
            }

            if (DepartmentId.HasValue)
            {
                query = query.Where(e =>
                    e.DepartmentEmployees.Any(de =>
                        de.DepartmentId == DepartmentId.Value));
            }

            if (StatusFilter.HasValue)
            {
                query = query.Where(e => e.Status == StatusFilter.Value);
            }

            if (!isHRAdmin)
            {
                query = query.Where(e => e.EmailAddress == currentUser.Email);
            }

            Employees = await query
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
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeactivateAsync(int id)
        {
            if (!User.IsInRole("HRAdmin"))
                return Forbid();

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.Status = Status.Inactive;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostActivateAsync(int id)
        {
            if (!User.IsInRole("HRAdmin"))
                return Forbid();

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.Status = Status.Active;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}