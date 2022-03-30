using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.Model
{
    public class UserInfoModel : IdentityUser
    {
        public string Name { get; set; }
    }
}
