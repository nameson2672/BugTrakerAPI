namespace BugTrakerAPI.Model.ReturnModel
{
    public class TeamResponse : CommonResponse
    {
        public TeamResponseData? data {get; set;}
    }
    public class TeamsByCreatorResponse : CommonResponse{
        public List<TeamResponseData>? data {set; get;}
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
}