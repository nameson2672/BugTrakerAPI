using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BugTrakerAPI.Model;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using BugTrakerAPI.ViewModel;
using BugTrakerAPI.Helper;

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
            var response = new RoleAddRes();
            response.success = false;
            if(String.IsNullOrEmpty(newRoleFromRequest)){
                response.errors = new List<string>(){"provide a valid role input"};
                return BadRequest(response);
            } 

            var doesRolePreExist = await _roleManager.RoleExistsAsync(newRoleFromRequest);
            if(doesRolePreExist) {
                 response.errors = new List<string>(){"role already exists"};
                return BadRequest(response);
            }
            var roleModel  = new IdentityRole(newRoleFromRequest);
            var createdRole = await _roleManager.CreateAsync(roleModel);
            
            if(!createdRole.Succeeded) {
                 response.errors = new SystemErrorParser().IdentityErrorParser(createdRole.Errors);

                 return BadRequest(response);
            }
            response.success = true;
            response.role = new RoleRes(){roleName=newRoleFromRequest, id=roleModel.Id};
            return Ok(response);
        }
        [HttpGet("GetAllRoles")]
        [AllowAnonymous]
        public IActionResult GetAllRoles(){
            var roles =  _db.Roles.ToList();
            var response = new RoleList();
            response.success = true;
            var litOfroles = new List<RoleRes>();
            foreach (IdentityRole role in roles){
                var rol = new RoleRes();
                rol.roleName = role.Name;
                rol.id = role.Id;
                litOfroles.Add(rol);
            }
            response.roles = litOfroles;
            return Ok(response);
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