using System.Security.Claims;
using AutoMapper;
using BugTrakerAPI.DatabaseTableModel;
using BugTrakerAPI.Model;
using BugTrakerAPI.Model.ReturnModel;
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
         private readonly IMapper _mapper;



        public TeamController(ApplicationDbContext db, UserManager<UserInfoModel> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> createTeam(TeamViewModel inputTeamInfo)
        {
            var response = new TeamResponse(){};
            if (!ModelState.IsValid){
                
            } 
                
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (String.IsNullOrEmpty(email)){
                response.errors = new List<string>(){"Invalid user input"};  
                return BadRequest(response);
            } 

            try
            {
                var user = await _userManager.FindByEmailAsync(email); 
                var teamCreated = _mapper.Map<Team>(inputTeamInfo);
                teamCreated.createrId = user.Id;
                var createdTeam =  _db.Team.Add(teamCreated);
                var teamAdmin = new TeamAdmin(){teamId = teamCreated.teamId, userId=user.Id};
                _db.TeamAdmins.Add(teamAdmin);
                await _db.SaveChangesAsync();
                response.data = _mapper.Map<TeamResponseData>(teamCreated);
                return Ok(response);
            }
            catch (Exception err)
            {
                response.errors = new List<string>(){err.Message};  
                return BadRequest(response);
            }

        }
        
        

    }
}