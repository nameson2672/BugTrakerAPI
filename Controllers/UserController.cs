using BugTrakerAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInfoModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.Password);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest();

        }
    }
}
