using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        public AdminController(DataContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;

        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRole()
        {
            var userList = await (from user in _context.Users
                                  orderby user.UserName
                                  select new
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in _context.Roles
                                               on userRole.RoleId equals role.Id
                                               select role.Name).ToList()
                                  }).ToListAsync();
            return Ok(userList);
        }

        [Authorize(Policy = "RequireModerator")]
        [HttpGet("photosForModeration")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admin or moderator can see this");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = roleEditDto.RoleNames;
            selectedRoles = selectedRoles ?? new string[]{};
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if(!result.Succeeded)
            {
                return BadRequest("Failed adding roles");
            }
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if(!result.Succeeded)
            {
                return BadRequest("Failed to remove roles");
            }
            return Ok(await _userManager.GetRolesAsync(user));
        }
    }
}