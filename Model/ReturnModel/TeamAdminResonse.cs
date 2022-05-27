using BugTrakerAPI.Model.ReturnModel;

namespace BugTrakerAPI.ViewModel
{
    public class SingleAdminAdded : CommonResponse
    {
        public TeamAdminRes data {get; set;}
    }
    public class AdminsInTeamResponse : CommonResponse
    {
        public List<TeamAdminRes> data {get; set;}
    }
    public class TeamAdminRes 
    {
        public string userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string? AvatarLink {get; set;}
        public string TeamRole {get; set;} = "Member";

               
        
    }
}