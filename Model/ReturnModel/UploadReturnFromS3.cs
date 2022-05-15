namespace BugTrakerAPI.Model.ReturnModel
{
    public class UploadReturnFromS3
    {
        public bool Sucess {get; set;}
        public string? AvatarLink {get; set;}
        public string? Error {get; set;}
    }
}