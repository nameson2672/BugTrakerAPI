using BugTrakerAPI.Helper;
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
        /// <summary>
        /// Create A new user
        /// </summary>
        /// <param name="student">Student Model</param>
        /// /// <remarks>
        /// Sample response by API:
        ///
        ///     POST /registor
        ///     {
        ///        "status": true,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [AllowAnonymous]
        [HttpPost("registor")]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {
             LoginRes User = new()
                {
                 success = false,
                    errors = null,
                    data = null
                };
            if (ModelState.IsValid)
            {
               
                if (await _userManager.FindByNameAsync(user.Email) == null)
                {
                    UserInfoModel userInfo = new UserInfoModel();
                    userInfo.Id = Guid.NewGuid().ToString();
                    userInfo.Email = user.Email;
                    userInfo.PhoneNumber = user.PhoneNumber;
                    userInfo.PasswordHash = user.Password;
                    userInfo.Name = user.Name;
                    userInfo.UserName = user.Email;

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
                        User.success = false;
                        var err = new SystemErrorParser();
                        User.errors = err.IdentityErrorParser(result.Errors);
                        
                        return BadRequest(User);
                    }
                }
                        User.success=false;
                        User.errors = new List<string> {"Email already registor try login"};

                return BadRequest(User);

            }
            User.success = false;
            User.errors = new List<string> { "Provide all information"};
            return BadRequest(User);

        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                
                var userInfoFromDatabase = await _userManager.FindByEmailAsync(user.Email);
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
