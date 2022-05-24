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
        [HttpGet("GetAllRoles")]
        [AllowAnonymous]
        public IActionResult GetAllRoles(){
            var roles =  _db.Roles.ToList();
            return Ok(roles);
        }
        [HttpPost("GiveMeARole")]
        [AllowAnonymous]
        public async Task<IActionResult> AssignRoleTOUser(string email, string roleName){
            if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(roleName)) return BadRequest("provide valid email or role name");
            var user = await _userManager.FindByEmailAsync(email);
            var doesRoleExists = await _roleManager.RoleExistsAsync(roleName);
            if(user == null || !doesRoleExists) return BadRequest("user is not a valid user");
            var roleAssigned = await _userManager.AddToRoleAsync(user, roleName);
            if(!roleAssigned.Succeeded) return BadRequest(roleAssigned.Errors.ToList());
            return Ok("role assigned to user");
        }
        [HttpPost("GetRoleSOfUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoles(string email){
            if(String.IsNullOrEmpty(email)) return BadRequest("Provide valid input");
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) return BadRequest("User not found");
            var rolesOfUser = await _userManager.GetRolesAsync(user);
            return Ok(rolesOfUser);
        }
          
    }
}