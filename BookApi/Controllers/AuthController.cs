using BookApi.Core.Data.Entities;
using BookApi.Core.DTOs;
using BookApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager) 
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _authService.Login(user, model.Password);
                    if (result != null && result.Succeeded)
                    {
                        var userToReturn = new ReturnUserDto
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                        };

                        var userroles = await _userManager.GetRolesAsync(user);
                        //var userclaims = await _userManager.GetClaimsAsync(user);

                        var token = _authService.GenerateJWT(user, userroles);

                        return Ok(new { user = userToReturn, token = token });
                    }
                    ModelState.AddModelError("Password", "Invalid Credentials");
                    return BadRequest(ModelState);
                }
                return BadRequest("Invalid credential");
            }
            
            return BadRequest(ModelState);
        }
    }
}
