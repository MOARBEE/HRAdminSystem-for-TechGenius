using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HRAdminSystem.ViewModels;

namespace HRAdminSystem.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            Input = new LoginViewModel();
            System.Diagnostics.Debug.WriteLine("LoginModel constructor called");
        }

        [BindProperty]
        public LoginViewModel Input { get; set; }

        public void OnGet()
        {
            System.Diagnostics.Debug.WriteLine("OnGet called");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            System.Diagnostics.Debug.WriteLine("=== Login Process Started ===");
            System.Diagnostics.Debug.WriteLine($"Email attempting login: {Input?.Email}");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email,
                    Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    System.Diagnostics.Debug.WriteLine("Login successful");
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                System.Diagnostics.Debug.WriteLine("Invalid login attempt");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Model Error: {error.ErrorMessage}");
                    }
                }

                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine(modelError.ErrorMessage);
                }
            }

            return Page();
        }
    }
}
