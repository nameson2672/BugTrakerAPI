using BugTrakerAPI.Model;
using BugTrakerAPI.Model.ReturnModel;
using BugTrakerAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUsernameAvaiability(string username)
        {
            if (username.Length > 0)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return Ok(username);
                }
                else
                {
                    return Ok("username not available");
                }
            }


            return BadRequest("provide the username");

        }
        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {
             LoginRes User = new()
                {
                    status = false,
                    errors = null,
                    data = null
                };
            if (ModelState.IsValid)
            {
               
                if (await _userManager.FindByNameAsync(user.UserName) == null)
                {
                    UserInfoModel userInfo = new UserInfoModel();
                    userInfo.Id = Guid.NewGuid().ToString();
                    userInfo.Email = user.Email;
                    userInfo.PhoneNumber = user.PhoneNumber;
                    userInfo.PasswordHash = user.Password;
                    userInfo.Name = user.Name;
                    userInfo.UserName = user.UserName;

                    var result = await _userManager.CreateAsync(userInfo, user.Password);
                    if (result.Succeeded)
                    {
                        User.data = new LoginCred()
                        {
                            Token = CreateToken(userInfo.Id.ToString()),
                            Name = userInfo.Name,
                            PhoneNumber = userInfo.PhoneNumber,
                            Email = userInfo.Email
                        };

                        return Ok(User);
                    }
                    else
                    {
                        
                        User.errors = result.Errors.ToString();
                        
                        return BadRequest(User);
                    }
                }
                
                        User.errors = "Username already Taken";
                       
                return BadRequest(User);

            }

            User.errors = "Provide all the information";
            return BadRequest(User);

        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserInfoModel userInfo = new UserInfoModel();
                userInfo.Id = Guid.NewGuid().ToString();
                userInfo.Email = user.Email;
                userInfo.PhoneNumber = user.PhoneNumber;
                /* userInfo.ConformPassword = user.ConformPassword;
                 userInfo.Password = user.Password;*/
                userInfo.Name = user.Name;
                userInfo.UserName = user.UserName;
                var userInfoFromDatabase = await _userManager.FindByEmailAsync(userInfo.Email);
                if (userInfoFromDatabase != null)
                {
                    var signInuser = await _signInManager.CheckPasswordSignInAsync(userInfoFromDatabase, user.Password, false);
                    if (signInuser.Succeeded)
                    {
                        return Ok(signInuser);
                    }


                    return BadRequest("username or password doesnt match");
                }
                return BadRequest("user doesn't esixt");


            }

            // LoginRes login = new(){
            //     status=true,
            //     errorMsg="",
            //     data = new List<LoginCred>(){
            //        new LoginCred(){
            //            Id="asxasd",
            //            Name="Nameson",
            //            Role="admin" 
            //        }

            //     }
            // };
            return BadRequest("login");
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
