using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Invalid role name provided! Role name must not be empty or null");

            var identityRole = new IdentityRole { Name = roleName };
            var roleResult = await _roleManager.CreateAsync(identityRole);

            if (roleResult.Succeeded)
            {
                return Ok("New role added!");
            }

            var errorList = new List<string>();
            foreach (var err in roleResult.Errors)
            {
                errorList.Add(err.Description);
            }

            return BadRequest(errorList);
        }
    }
}
