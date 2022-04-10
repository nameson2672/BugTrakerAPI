using BugTrakerAPI.Helper;
using BugTrakerAPI.Model;
using BugTrakerAPI.Model.ReturnModel;
using BugTrakerAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<UserInfoModel> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtConfig _jwtConfig;
        private readonly SignInManager<UserInfoModel> _signInManager;
        private readonly ApplicationDbContext _apiDbContext;
        public UserController(
            UserManager<UserInfoModel> userManager,
            IConfiguration configuration,
            SignInManager<UserInfoModel> signInManager,
            ApplicationDbContext apiDbContext,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _apiDbContext = apiDbContext;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;

        }
        /// <summary>
        /// Create A new user
        /// </summary>
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
                        var tokensForUser = await CreateToken(userInfo);
                        User.data = new LoginCred()
                        {
                            Token = tokensForUser.Token,
                            RefreshToken = tokensForUser.RefreshToken,
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
                User.success = false;
                User.errors = new List<string> { "Email already registor try login" };

                return BadRequest(User);

            }
            User.success = false;
            User.errors = new List<string> { "Provide all information" };
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
                        return Ok(new
                        {
                            success = true,
                            data = signInuser

                        });
                    }


                    return BadRequest(new
                    {
                        sucess = false,
                        errors = "Email Or Password doesn't Match"
                    });
                }
                return BadRequest(new
                {
                    sucess = false,
                    errors = "User doesn't exists"
                });


            };
            return BadRequest(new
            {
                sucess = false,
                errors = "Provide all informations"
            }
                    );
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetData(string username)
        {
            try
            {
                var userInfoFromDatabase = await _userManager.FindByEmailAsync(username);
                return Ok(userInfoFromDatabase);
            }
            catch
            {
                return BadRequest("error");
            }

        }
        private async Task<TokensResponse> CreateToken(UserInfoModel user)
        {
            
            return new TokensResponse{
                Token = "asdasd",
                RefreshToken = "asdasd"
            };
        }
        private async Task<List<Claim>> GetAllValidClaims(UserInfoModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Getting the claims that we have assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        // private async Task<LoginRes> VerifyAndGenerateToken(TokenRequest tokenRequest)
        // {
        //     var jwtTokenHandler = new JwtSecurityTokenHandler();



        // }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}

