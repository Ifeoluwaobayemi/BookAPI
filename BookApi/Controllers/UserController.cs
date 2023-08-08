using BookApi.Core.Data.Entities;
using BookApi.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
   

    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    //var result = await _userManager(user);
                    //if (result.Succeeded)
                    //{
                    //    foreach (var error in result.Errors)
                    //    {
                    //        ModelState.AddModelError(error.Code, error.Description);
                    //    }
                    //}
                    var userToReturn = new ReturnUserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    };

                    return Ok(userToReturn);
                }

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();
            var usersToReturn = new List<ReturnUserDto>();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    usersToReturn.Add(new ReturnUserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                    });
                }
            }

            return Ok(usersToReturn);
        }

        
        [HttpGet("single/{id}")]
        public async Task<IActionResult> GetSingle(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {

                var userToReturn = new ReturnUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };

                return Ok(userToReturn);

            }

            return NotFound($"No user was found with id: {id}");
        }

        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"No user was found with id: {id}");
            }

            var identityResult = await _userManager.DeleteAsync(user);
            if (identityResult.Succeeded)
            {
                return Ok("User deleted successfully.");
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        
        [HttpPost("add-user-role")]
        public async Task<IActionResult> AddUserRole([FromBody] AddUserRoleDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound($"Could not find user with id: {model.UserId}");
                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                return Ok("Role added to user successfully!");
            }
            return BadRequest(ModelState);
        }

        
        [HttpPost("get-user-role/{userId}")]
        public async Task<IActionResult> GetUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Could not find user with id: {userId}");

            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(userRoles);
        }
    }
}
