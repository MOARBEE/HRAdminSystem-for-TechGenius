using HRAdminSystem.Data;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HRAdminSystem.Enums;

namespace HRAdminSystem.Pages.Departments
{
    [Authorize(Policy = "HRAdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DepartmentListViewModel> Departments { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public HRAdminSystem.Enums.Status? StatusFilter { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Departments
                .Include(d => d.DepartmentEmployees)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(d => d.Name.Contains(SearchTerm));
            }

            if (StatusFilter.HasValue)
            {
                query = query.Where(d => d.Status == StatusFilter.Value);
            }

            Departments = await query
                .Select(d => new DepartmentListViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Status = d.Status,
                    EmployeeCount = d.DepartmentEmployees.Count
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeactivateAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.Status = HRAdminSystem.Enums.Status.Inactive;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostActivateAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.Status = HRAdminSystem.Enums.Status.Active;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
