using System.Security.Claims;
using BugTrakerAPI.DatabaseTableModel;
using BugTrakerAPI.Model;
using BugTrakerAPI.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserInfoModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public TeamController(ApplicationDbContext db, UserManager<UserInfoModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> createTeam(TeamViewModel inputTeamInfo)
        {
            if (!ModelState.IsValid) return BadRequest("provide a proper information");
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (String.IsNullOrEmpty(email)) return BadRequest("please login first");

            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var team = new Team() {teamId = Guid.NewGuid().ToString(),  teamName = inputTeamInfo.teamName, createrId = user.Id, workingOn = inputTeamInfo.workingOn, description = inputTeamInfo.description, mainFunctions = inputTeamInfo.mainFunctions, CreatedAt=DateTime.Now };

                var createdTeam =  _db.Team.Add(team);
                await _db.SaveChangesAsync();
                
                return Ok("createdTeam");
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }

    }
}