using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace System_Zarz.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ManageRolesModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ManageRolesModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<UserWithRoles> Users { get; set; } = new();

    [BindProperty]
    public string UserId { get; set; }

    [BindProperty]
    public string SelectedRole { get; set; }

    public string? StatusMessage { get; set; }

    public class UserWithRoles
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public IList<string> Roles { get; set; } = new List<string>();
    }

    public async Task OnGetAsync()
    {
        Users = new List<UserWithRoles>();

        foreach (var user in _userManager.Users.ToList())
        {
            var roles = await _userManager.GetRolesAsync(user);
            Users.Add(new UserWithRoles
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(SelectedRole))
        {
            StatusMessage = "Nieprawidłowe dane.";
            await OnGetAsync();
            return Page();
        }

        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            StatusMessage = "Użytkownik nie znaleziony.";
            await OnGetAsync();
            return Page();
        }

        // usuń poprzednie role (poza Adminem!)
        var existingRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in existingRoles)
        {
            if (role != "Admin") // nie usuwaj Admina
                await _userManager.RemoveFromRoleAsync(user, role);
        }

        await _userManager.AddToRoleAsync(user, SelectedRole);
        StatusMessage = $"Rola '{SelectedRole}' została przypisana do użytkownika {user.Email}.";

        await OnGetAsync();
        return Page();
    }
}
