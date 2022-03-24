using Microsoft.AspNetCore.Identity;

namespace BugTrakerAPI.Model
{
    public class UserInfoModel : IdentityUser
    {
        // public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; }
        public string ConformPassword { get; set; }

    }
}
