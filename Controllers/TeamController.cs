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
                response.errors = new List<string>() { error.Message };
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
                var newMbemberToTeam = new TeamMembers() { teamId = teamCreated.teamId, userId = user.Id, isUserAdmin = true};
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
                if (isUserAnMember.isUserAdmin)
                {
                    response.errors = new List<string>() { "the user you want to include make an admin is already an admin" };
                    return BadRequest(response);
                }
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
                var newMemberEntry = new TeamMembers() { teamId = inputValue.teamId, userId = inputValue.userId };
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
        [HttpPost("UpdateTeamInfo")]
        public async Task<IActionResult> UpdateTeamInfo(TeamUpdateViewModle teamInfoToBeupdated)
        {
            var response = new TeamResponse();
            var userId = User.Identities.ToList()[0].Claims.First().Value;
            if (!ModelState.IsValid)
            {
                response.errors = new List<string>() { "Please provide a valid information" };
                return BadRequest(response);
            }
            try
            {
                var team = _db.Team.Where(item => (item.teamId == teamInfoToBeupdated.teamId && item.createrId == userId)).ToList();
                if (team == null)
                {
                    response.errors = new List<string>() { "Team id is not valid" };
                    return Ok(response);
                }
                team[0].teamName = teamInfoToBeupdated.teamName == null ? team[0].teamName : teamInfoToBeupdated.teamName;
                team[0].description = teamInfoToBeupdated.description == null ? team[0].description : teamInfoToBeupdated.description;
                team[0].mainFunctions = teamInfoToBeupdated.mainFunctions == null ? team[0].mainFunctions : teamInfoToBeupdated.mainFunctions;
                team[0].workingOn = teamInfoToBeupdated.workingOn == null ? team[0].workingOn : teamInfoToBeupdated.workingOn;
                _db.Team.Update(team[0]);
                await _db.SaveChangesAsync();
                response.success = true;
                response.data = _mapper.Map<TeamResponseData>(team[0]);
                return Ok(response);
            }
            catch (Exception err)
            {
                response.errors = new List<string>() { err.Message };
                var result = new ObjectResult(new { statusCode = 500, response });
                return result;
            }
        }
        [HttpGet("GetTeamByID")]
        public IActionResult GetTeamByID(string id)
        {
            var response = new TeamInfoWithAllMemberResponse();
            if (String.IsNullOrEmpty(id))
            {
                response.errors = new List<string>() { "please provide valid id" };
                return BadRequest(response);
            }
            try
            {
                var teamInfo = _db.Team.Where(item => item.teamId == id).ToList()[0];
                if (teamInfo == null)
                {
                    response.errors = new List<string>() { "Team id is not valid" };
                    return BadRequest(response);
                }
                var membersInTeam = from team in _db.Team
                                    where team.teamId == id
                                    join members in _db.TeamMembers on team.teamId equals members.teamId
                                    join user in _db.Users on members.userId equals user.Id
                                    select user;
                response.data = _mapper.Map<TeamWithAllMemberInfo>(teamInfo);
                var mamberList = new List<TeamMemberOrTeamAdminInfoModel>();
                foreach (UserInfoModel item in membersInTeam.ToList()){
                    mamberList.Add(_mapper.Map<TeamMemberOrTeamAdminInfoModel>(item));
                }
                    response.data.membersWithRole=mamberList;
                return Ok(response);
            }
            catch (Exception err)
            {
                response.errors = new List<string>() { err.Message };
                var result = new ObjectResult(new { statusCode = 500, response });
                return result;
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Test()
        {
            //var forprog = _db.Team.Where(item=>item.createrId == "27787442-1357-45c6-966b-bcf0229358cf").Join(inner:)
            var dataQuery = from team in _db.Team
                            where team.createrId == "27787442-1357-45c6-966b-bcf0229358cf"
                            join members in _db.TeamMembers on team.teamId equals members.teamId
                            join user in _db.Users on members.userId equals user.Id
                            select new { teamInfo = team, users = user };
            var result = dataQuery.ToArray();
            return Ok(result);
        }


    }
}