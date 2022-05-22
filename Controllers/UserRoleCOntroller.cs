using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BugTrakerAPI.Model;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserRoleCOntroller : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserInfoModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UserRoleCOntroller(ApplicationDbContext db, UserManager<UserInfoModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("PostRole")]
        [AllowAnonymous]
        public async Task<IActionResult> createRole(string newRoleFromRequest){
            if(String.IsNullOrEmpty(newRoleFromRequest)) return BadRequest("give input role to create");

            var doesRolePreExist = await _roleManager.RoleExistsAsync(newRoleFromRequest);
            if(doesRolePreExist) return BadRequest("Role Already exixts");

            var createdRole = await _roleManager.CreateAsync(new IdentityRole(newRoleFromRequest));
            
            if(!createdRole.Succeeded) return BadRequest("Faild to create role.");
            return Ok("Created Sucessfully");
        }
       

    }
}