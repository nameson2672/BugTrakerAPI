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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BugTrakerAPI.Services;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Web;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<UserInfoModel> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtConfig _jwtConfig;
        private readonly SignInManager<UserInfoModel> _signInManager;
        private readonly ApplicationDbContext _apiDbContext;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IMailSender _sendMail;
        public UserController(
            UserManager<UserInfoModel> userManager,
            IConfiguration configuration,
            SignInManager<UserInfoModel> signInManager,
            ApplicationDbContext apiDbContext,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            IMailSender sendMail
            )
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _apiDbContext = apiDbContext;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParameters;
            _sendMail = sendMail;
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
        public async Task<IActionResult> LoginUser(LoginViewModel user)
        {
            var response = new LoginRes();
            if (ModelState.IsValid)
            {

                var dbUser = await _userManager.FindByEmailAsync(user.Email);
                if (dbUser != null)
                {
                    var signInuser = await _signInManager.CheckPasswordSignInAsync(dbUser, user.Password, false);
                    if (signInuser.Succeeded)
                    {
                        var newlyFormTokens = await CreateToken(dbUser);
                        response.success = true;
                        response.data = new LoginCred
                        {
                            Name = dbUser.Name,
                            Email = dbUser.Email,
                            PhoneNumber = dbUser.PhoneNumber,
                            Token = newlyFormTokens.Token,
                            RefreshToken = newlyFormTokens.RefreshToken
                        };
                        return Ok(response);
                    }

                    response.success = false;
                    response.errors = new List<string> { "Email or Password doesn't match" };
                    return BadRequest(response);
                }

                response.success = false;
                response.errors = new List<string> { "User dosen't exists" };
                return BadRequest(response);


            };

            response.success = false;
            response.errors = new List<string> { "invalid crediantial" };
            return BadRequest(response);
        }

        [HttpPost("getTokens")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokens(TokenRequest inputToken)

        {
            var response = new TokensResponse();
            if (ModelState.IsValid)
            {
                var newTokens = await VerifyAndGenerateToken(inputToken);
                if (newTokens.success)
                {
                    response.success = true;
                    response.Token = newTokens.Token;
                    response.RefreshToken = newTokens.RefreshToken;
                    return Ok(response);
                }

                response.success = false;
                response.errors = newTokens.errors;
                return BadRequest(response);

            }
            response.success = false;
            response.errors = new List<string> { "Invlaid crediantial" };
            return BadRequest(response);


        }
        [HttpPost("getEmailVerify")]
        public async Task<IActionResult> MailVerify()
        {
            EmailResponse response = new()
            {
                success = false
            };
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var emailConformationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                MailModel mail = new();
                mail.toemail = user.Email;
                mail.toname = user.Name;
                mail.subject = "Verify Email";
                var url = "https://localhost:7186/api/User/verify?id=" + user.Id + "&code=" + emailConformationToken;
                var parseUrl = Url.Action("VerifyCodeFromMail", "User", new { id = user.Id, code = emailConformationToken }, protocol: HttpContext.Request.Scheme);
                var urlRedirect = "To verify the mail tou provided click on <a href=" + url + ">Link</a>";
                mail.message = urlRedirect;
                
                //_sendMail.SendMail(mail);
                return Ok(new {url = url, parseUrl = parseUrl});
            }
            response.errors = new List<string> { "User not found." };
            return Ok(response);

        }
        [HttpGet("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCodeFromMail(string id, string code)
        {
            EmailResponse response = new();
            response.success = false;
            
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var parsecode = Uri.EscapeDataString(code);
                    var decode = Uri.UnescapeDataString(code);
                    var result = await _userManager.ConfirmEmailAsync(user, code);
                    response.success = true;
                    
                    response.Message = "Email Verified.";
                    return Ok(response);
                }
                response.errors = new List<string> { "User can't found." };
                return Ok(response);
            
            response.errors = new List<string> { "Code or user id is null." };
            return Ok(response);
        }
        [HttpGet("GoogleApiCall")]
        public OkObjectResult GoogleCall()
        {
            return Ok("Nameson Gaudel");
        }
        private async Task<TokensResponse> CreateToken(UserInfoModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(55),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new TokensResponse
            {
                success = true,
                Token = jwtToken,
                RefreshToken = refreshToken.Token
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

        private async Task<TokensResponse> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            TokensResponse response = new TokensResponse { };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {

                // Validate JWT token   --1
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validateToken);

                // Validate encryption algorithm
                if (validateToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        response.success = false;
                        response.errors = new List<string> { "Token is not valid please re login and try again" };
                        return response;
                    }
                }

                //validate existatnce of the token
                var storedToken = await _apiDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    response.success = false;
                    response.errors = new List<string> { "Token doesn't exist" };
                    return response;
                }

                // validate if token is used
                if (storedToken.IsUsed)
                {
                    response.success = false;
                    response.errors = new List<string> { "Token is already used." };
                    return response;
                }

                // calidate if revoked
                if (storedToken.IsRevorked)
                {
                    response.success = false;
                    response.errors = new List<string> { "Token has been revoked" };
                    return response;
                }

                // validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (storedToken.JwtId != jti)
                {
                    response.success = false;
                    response.errors = new List<string> { "Token does not match" };
                    return response;
                }

                storedToken.IsUsed = true;
                _apiDbContext.RefreshTokens.Update(storedToken);
                await _apiDbContext.SaveChangesAsync();

                // generate a new JWT and refresh token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await CreateToken(dbUser);

            }
            catch (Exception error)
            {
                if (error.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    response.success = false;
                    response.errors = new List<string> { "Token has expired please re-login" };
                    return response;



                }
                else
                {
                    response.success = false;
                    response.errors = new List<string> { "Something went wrong." };
                    return response;

                };
            }



        }

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