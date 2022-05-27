using System.Security.Claims;
using AutoMapper;
using BugTrakerAPI.Attributes;
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
        [HttpGet("GetMyTeams")]
        public IActionResult GetMyTeams()
        {
            var response = new TeamsByCreatorResponse();
            var userId = User.Identities.ToList()[0].Claims.First().Value;
            try
            {
                var teams = _db.Team.Where(item => item.createrId == userId).ToList();
                var listTeam = new List<TeamResponseData>();
                foreach (Team team in teams)
                {
                    var mapped = _mapper.Map<TeamResponseData>(team);
                    listTeam.Add(mapped);
                }
                response.data = listTeam;
                response.success = true;
                return Ok(response);
            }
            catch (Exception error)
            {
                response.errors = new List<string>(){error.Message};
                return BadRequest(response);
            }


        }
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> createTeam(TeamViewModel inputTeamInfo)
        {
            var response = new TeamResponse() { };
            response.success = false;
            if (!ModelState.IsValid)
            {
                response.errors = new List<string>() { "Provide all the information needed." };
                return BadRequest(response);
            }

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (String.IsNullOrEmpty(email))
            {
                response.errors = new List<string>() { "Invalid user input" };
                return BadRequest(response);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var teamCreated = _mapper.Map<Team>(inputTeamInfo);
                teamCreated.createrId = user.Id;
                var createdTeam = _db.Team.Add(teamCreated);
                var teamAdmin = new TeamAdmin() { teamId = teamCreated.teamId, userId = user.Id };
                var newMbemberToTeam = new TeamMembers() { TeamId = teamCreated.teamId, UserId = user.Id };
                _db.TeamAdmins.Add(teamAdmin);
                _db.TeamMembers.Add(newMbemberToTeam);
                await _db.SaveChangesAsync();
                response.success = true;
                response.data = _mapper.Map<TeamResponseData>(teamCreated);
                return Ok(response);
            }
            catch (Exception err)
            {
                response.errors = new List<string>() { err.Message };
                return BadRequest(response);
            }

        }
        [TeamAdmin("inputValue")]
        [HttpPost("changeMamberToAdmin")]
        public async Task<IActionResult> AddToTeamAsAdmin(AddUserToTeam inputValue)
        {
            var response = new SingleAdminAdded();
            if (!ModelState.IsValid)
            {
                response.errors = new List<string>() { "model state is not valid" };
                return BadRequest(response);
            }
            try
            {
                var userTobeIncludedInTeam = await _userManager.FindByIdAsync(inputValue.userId);
                var isUserAnMember = _db.TeamMembers.Find(inputValue.teamId, inputValue.userId);
                if (userTobeIncludedInTeam == null || isUserAnMember == null)
                {
                    response.errors = new List<string>() { "the user you want to include in team doesen't exists or is not a member" };
                    return BadRequest(response);
                }
                var isUserAlreadyAdmin = _db.TeamAdmins.Find(inputValue.teamId, inputValue.userId);
                if (isUserAlreadyAdmin != null)
                {
                    response.errors = new List<string>() { "the user you want to include in team doesen't exists or is not a member" };
                    return BadRequest(response);
                }
                var newAdminEntry = new TeamAdmin() { teamId = inputValue.teamId, userId = inputValue.userId };
                var addedToTeam = _db.TeamAdmins.Add(newAdminEntry);
                await _db.SaveChangesAsync();
                response.data = _mapper.Map<TeamAdminRes>(userTobeIncludedInTeam);
                response.success = true;
                return Ok(response);
            }
            catch (Exception error)
            {
                response.errors = new List<string>() { error.Message };
                return BadRequest(response);
            }
        }
        [TeamAdmin("inputValue")]
        [HttpPost("addMemberToTeam")]
        public async Task<IActionResult> AddAsAMember(AddUserToTeam inputValue)
        {
            var response = new SingleAdminAdded();
            if (!ModelState.IsValid)
            {
                response.errors = new List<string>() { "model state is not valid" };
                return BadRequest(response);
            }
            try
            {
                var userTobeIncludedInTeam = await _userManager.FindByIdAsync(inputValue.userId);
                if (userTobeIncludedInTeam == null)
                {
                    response.errors = new List<string>() { "the user you want to include in team doesen't exists" };
                    return BadRequest(response);
                }
                var isUserAnMember = _db.TeamMembers.Find(inputValue.teamId, inputValue.userId);
                if (isUserAnMember != null)
                {
                    response.errors = new List<string>() { "User is already a member" };
                    return BadRequest(response);
                }
                var newMemberEntry = new TeamMembers() { TeamId = inputValue.teamId, UserId = inputValue.userId };
                var addedToTeam = _db.TeamMembers.Add(newMemberEntry);
                await _db.SaveChangesAsync();
                response.data = _mapper.Map<TeamAdminRes>(userTobeIncludedInTeam);
                response.success = true;
                return Ok(response);
            }
            catch (Exception error)
            {
                response.errors = new List<string>() { error.Message };
                return BadRequest(response);
            }
        }
    }
}