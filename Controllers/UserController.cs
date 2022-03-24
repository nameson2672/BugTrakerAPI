using BugTrakerAPI.Model;
using BugTrakerAPI.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;

        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserInfoModel userInfo = new UserInfoModel();
                userInfo.Id = Guid.NewGuid().ToString();
                userInfo.Email = user.Email;
                userInfo.PhoneNumber = user.PhoneNumber;
                userInfo.ConformPassword = user.ConformPassword;
                userInfo.Password = user.Password;
                userInfo.UserName = user.Name;

                var result = await _userManager.CreateAsync(userInfo, userInfo.Password);
                if (result.Succeeded)
                {

                    return Ok(CreateToken(userInfo.Id.ToString()));
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest();

        }
        private string CreateToken(string id)
        {
            //Create a List of Claims, Keep claims name short    
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", id));
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
