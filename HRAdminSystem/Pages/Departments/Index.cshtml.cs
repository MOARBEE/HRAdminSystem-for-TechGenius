using HRAdminSystem.Data;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public async Task OnGetAsync()
        {
            Departments = await _context.Departments
                .Include(d => d.DepartmentEmployees)
                .Select(d => new DepartmentListViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Status = d.Status,
                    EmployeeCount = d.DepartmentEmployees.Count
                })
                .ToListAsync();
        }
    }
}
