namespace BugTrakerAPI.Model.ReturnModel
{
    public class TeamResponse
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