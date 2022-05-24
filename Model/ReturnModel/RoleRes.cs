using BugTrakerAPI.Model.ReturnModel;

namespace BugTrakerAPI.ViewModel
{
    public class RoleAddRes : CommonResponse
    {
        public RoleRes role {get; set;}
        
    }
    public class RoleList : CommonResponse
    {
        public List<RoleRes> roles {get; set;}
    }
    public class RoleRes 
    {
        public string id {get; set;}
        public string roleName { get; set; }
    }
}