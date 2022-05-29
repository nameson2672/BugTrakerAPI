namespace BugTrakerAPI.Model.ReturnModel
{
    public class TeamResponse : CommonResponse
    {
        public TeamResponseData? data {get; set;}
    }
    public class TeamsByCreatorResponse : CommonResponse{
        public List<TeamResponseData>? data {set; get;}
    }
    public class TeamInfoWithAllMemberResponse : CommonResponse{
        public TeamWithAllMemberInfo? data {get;set;}
    }
    public class AvailableTeamsResponse :CommonResponse
    {
        public List<TeamResponseWithCreaterData>? data {get; set;}
    }
    
    
    public class TeamResponseData 
    {
        public string teamId {get; set;}
        public string teamName {get; set;}
        public string workingOn {get; set;}
        public string mainFunctions {get; set;}
        public string description {get; set;}
        public string? coverImageLink {get; set;}
        public string? teamAvatar {get; set;}
        public string createrId {get; set;}
    }
    public class TeamResponseWithCreaterData {
        public string teamId {get; set;}
        public string teamName {get; set;}
        public string workingOn {get; set;}
        public string mainFunctions {get; set;}
        public string description {get; set;}
        public string? coverImageLink {get; set;}
        public string? teamAvatar {get; set;}
        public TeamMemberOrTeamAdminInfoModel? createrInfo {get; set;}
    }
    public class TeamWithAllMemberInfo {
        public string teamId {get; set;}
        public string teamName {get; set;}
        public string workingOn {get; set;}
        public string mainFunctions {get; set;}
        public string description {get; set;}
        public string? coverImageLink {get; set;}
        public string? teamAvatar {get; set;}
        public List<TeamMemberOrTeamAdminInfoModel>? membersWithRole {get; set;}

    }
     public class TeamMemberOrTeamAdminInfoModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? AvatarLink {get; set;}
        public string roleInTeam {get; set;} = "Member";
    }
}