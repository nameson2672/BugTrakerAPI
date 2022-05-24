using BugTrakerAPI.DatabaseTableModel;
using BugTrakerAPI.ViewModel;

namespace BugTrakerAPI.Model.ReturnModel
{
    public class UserReturnModel : CommonResponse
    {
        public UserInfoReturn? data {get; set;}
    }

    public class UserInfoReturn 
    {
    public string id {get; set;}
    public string name {get; set;}
    public string Email {get; set;}
    public string? PhoneNumber {get; set;}
    public List<Team>? teamOwn {get; set;}
    public List<RoleRes>? roles {get; set;}
    }
}