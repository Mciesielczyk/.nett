using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace System_Zarz.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ManageRolesController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ManageRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public class UserWithRoles
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public IList<string> Roles { get; set; } = new List<string>();
    }

    public class ChangeRoleRequest
    {
        public string UserId { get; set; } = "";
        public string SelectedRole { get; set; } = "";
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserWithRoles>>> GetUsersWithRoles()
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserWithRoles>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserWithRoles
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Roles = roles
            });
        }

        return Ok(result);
    }

    [HttpPost("change-role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleRequest request)
    {
        if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.SelectedRole))
        {
            return BadRequest("Nieprawidłowe dane.");
        }

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return NotFound("Użytkownik nie znaleziony.");
        }

        var existingRoles = await _userManager.GetRolesAsync(user);

        // Usuń wszystkie role poza Adminem
        foreach (var role in existingRoles)
        {
            if (role != "Admin")
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(user, role);
                if (!removeResult.Succeeded)
                {
                    return StatusCode(500, $"Błąd przy usuwaniu roli {role}.");
                }
            }
        }

        var addResult = await _userManager.AddToRoleAsync(user, request.SelectedRole);
        if (!addResult.Succeeded)
        {
            return StatusCode(500, $"Błąd przy dodawaniu roli {request.SelectedRole}.");
        }

        return Ok($"Rola '{request.SelectedRole}' została przypisana do użytkownika {user.Email}.");
    }
}
