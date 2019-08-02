using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;
        public AuthController(IAuthRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto){
            ////validating request

            registerDto.Username=registerDto.Username.ToLower();
            if(await _repo.UserExists(registerDto.Username)) return BadRequest("User already exists!");

            var userToCreate=new User {
                Username=registerDto.Username
            };
            var createdUser = await _repo.Register(userToCreate,registerDto.Password);
            return StatusCode(201);
        }
    }
}